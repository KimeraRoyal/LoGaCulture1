using System.Collections.Generic;
using System.Linq;
using KW.Flags;
using Mapbox.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

/// Serializable container for a Save Point's data. 
/// All data is stored as strings, and the only concrete game class it depends on is the SaveData component.
[System.Serializable]
public class SavePointData
{
    [SerializeField] protected string savePointKey;
    [SerializeField] protected string savePointDesc;
    [SerializeField] protected string sceneName;
    [SerializeField] protected List<SaveDataItem> saveDataItems = new List<SaveDataItem>();
    
    [SerializeField] protected List<uint> flagBits;

    [SerializeField] protected List<string> activeMarkers;

    protected static SavePointData Create(string _savePointKey, string _savePointDesc, string _sceneName, List<uint> _flagBits, List<string> _activeMarkers)
    {
        var savePointData = new SavePointData();

        savePointData.savePointKey = _savePointKey;
        savePointData.savePointDesc = _savePointDesc;
        savePointData.sceneName = _sceneName;

        savePointData.flagBits = _flagBits;

        savePointData.activeMarkers = _activeMarkers;

        return savePointData;
    }

    public string SavePointKey { get { return savePointKey; } set { savePointKey = value; } }
    public string SavePointDesc { get { return savePointDesc; } set { savePointDesc = value; } }
    public string SceneName { get { return sceneName; } set { sceneName = value; } }
    
    public List<uint> FlagBits
    {
        get => flagBits;
        set => flagBits = value;
    }

    public List<SaveDataItem> SaveDataItems { get { return saveDataItems; } }

    /// Encodes a new Save Point to data and converts it to JSON text format.
    public static string Encode(string _savePointKey, string _savePointDesc, string _sceneName)
    {
        var flags = Object.FindAnyObjectByType<Flags>();

        var spawnOnMap = Object.FindAnyObjectByType<SpawnOnMap>();
        var activeMarkers = (from marker in spawnOnMap.Markers.List where marker.gameObject.activeSelf select marker.ID).ToList();

        var savePointData = Create(_savePointKey, _savePointDesc, _sceneName, flags.FlagBits, activeMarkers);
        var saveData = GameObject.FindObjectOfType<SaveData>();
        if(saveData != null)
        {
            saveData.Encode(savePointData.saveDataItems);
        }
        return JsonUtility.ToJson(savePointData, true);
    }

    /// Decodes a Save Point from JSON text format and loads it.
    public static void Decode(string saveDataJSON)
    {
        var savePointData = JsonUtility.FromJson<SavePointData>(saveDataJSON);

        UnityAction<Scene, LoadSceneMode> onSceneLoadedAction = null;

        onSceneLoadedAction = (scene, mode) =>
        {
            // Additive scene loads and non-matching scene loads could happen if the client is using the SceneManager directly
            // Directly ignoring them is the best practice
            if (mode == LoadSceneMode.Additive || scene.name != savePointData.SceneName)
            {
                return;
            }

            SceneManager.sceneLoaded -= onSceneLoadedAction;

            // Look for the SaveData component in the scene and decode the Save Point data
            var saveData = GameObject.FindObjectOfType<SaveData>();
            if (saveData != null)
            {
                saveData.Decode(savePointData.saveDataItems);
            }
            
            var flags = Object.FindAnyObjectByType<Flags>();
            flags.FlagBits = savePointData.flagBits;

            var flowEngine = Object.FindAnyObjectByType<BasicFlowEngine>();
            var spawnOnMap = Object.FindAnyObjectByType<SpawnOnMap>();

            foreach (var markerID in savePointData.activeMarkers)
            {
                var locationVariable = flowEngine.Variables.Find(variable => variable.Key == markerID);
                spawnOnMap.ShowLocationMarker((LocationVariable)locationVariable);
            }

            SaveManagerSignals.DoSavePointLoaded(savePointData.savePointKey);
        };

        SceneManager.sceneLoaded += onSceneLoadedAction;
        SceneManager.LoadScene(savePointData.SceneName);
    }
}
