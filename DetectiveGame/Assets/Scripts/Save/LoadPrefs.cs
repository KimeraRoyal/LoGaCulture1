using Save;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadPrefs : MonoBehaviour
{
    public void Load()
    {
        if (!PlayerPrefs.HasKey("scene")) { return; }

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
        try
        {
            SceneManager.LoadScene(sceneName);
        }
        catch
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            throw;
        }
    }

    private void LoadInScene()
    {
        var savers = FindObjectsOfType<Saver>();
        foreach (var saver in savers)
        {
            try
            {
                if (saver != null)
                {
                    saver.Load();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load saver: {e.Message}");
            }
        }
    }
}