using System;
using System.Collections;
using System.Collections.Generic;
using Save;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadPrefs : MonoBehaviour
{
    public void Load()
    {
        if(!PlayerPrefs.HasKey("scene")) { return; }

        var sceneName = PlayerPrefs.GetString("scene");
        /*if (SceneManager.GetActiveScene().name == sceneName)
        {
            LoadInScene();
            return;
        }*/

        /*void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            LoadInScene();
        }

        SceneManager.sceneLoaded += OnSceneLoaded;*/
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string _sceneName)
    {
        var asyncLoad = SceneManager.LoadSceneAsync(_sceneName);
        
        if (asyncLoad == null) { yield break; }
    
        while (!asyncLoad.isDone)
        { 
            yield return null;
        }
        
        LoadInScene();
    }

    private void LoadInScene()
    {
        var savers = FindObjectsOfType<Saver>();
        foreach (var saver in savers)
        {
            saver.Load();
        }
    }
}