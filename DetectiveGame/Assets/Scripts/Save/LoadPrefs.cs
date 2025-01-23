using System;
using Save;
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

        void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            LoadInScene();
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
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