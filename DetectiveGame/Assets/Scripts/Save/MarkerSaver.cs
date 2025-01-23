using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mapbox.Examples;
using Save;
using UnityEngine;

[RequireComponent(typeof(SpawnOnMap))]
public class MarkerSaver : Saver
{
    public override void Save()
    {
        var spawner = GetComponent<SpawnOnMap>();
        
        var activeMarkers = (from marker in spawner.Markers.List where marker.gameObject.activeSelf select marker.ID).ToList();
        
        for (var i = 0; i < activeMarkers.Count; i++)
        {
            PlayerPrefs.SetString("markerIDs" + i, activeMarkers[i]);
        }
    }

    public override void Load()
    {
        if(!PlayerPrefs.HasKey("markerArraySize")) { return; }
        
        var markerArraySize = PlayerPrefs.GetInt("markerArraySize");
        if(markerArraySize < 1) { return; }
        
        var spawner = GetComponent<SpawnOnMap>();
        var locationVariables = new List<LocationVariable>();

        var flowEngine = spawner.GetComponentInParent<BasicFlowEngine>();
        for (var i = 0; i < markerArraySize; i++)
        {
            var markerID = PlayerPrefs.GetString("markerIDs" + i);
            
            var location = flowEngine.Variables.Find(variable => variable.Key == markerID);
            locationVariables.Add((LocationVariable)location);
        }

        StartCoroutine(InitialiseMarkers(spawner, locationVariables));
    }

    private IEnumerator InitialiseMarkers(SpawnOnMap _spawner, List<LocationVariable> _variables)
    {
        yield return new WaitUntil(() => _spawner.Initialised);
        foreach (var variable in _variables)
        {
            _spawner.ShowLocationMarker(variable);
        }
    }
}
