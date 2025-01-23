using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadPrefs : MonoBehaviour
{
    public Action OnLoad;
    
    public void Load()
    {
        if(!PlayerPrefs.HasKey("scene")) { return; }

        var sceneName = PlayerPrefs.GetString("scene");
        /*if (SceneManager.GetActiveScene().name == sceneName)
        {
            LoadInScene();
            return;
        }*/

        void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            
            Debug.Log($"Scene {sceneName} Loaded!");
            LoadInScene();
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    private void LoadInScene()
    {
        OnLoad?.Invoke();
        
        Debug.Log("Loaded!");
    }
}