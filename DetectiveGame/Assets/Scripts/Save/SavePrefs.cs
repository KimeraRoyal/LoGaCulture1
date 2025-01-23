using System;
using Save;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePrefs : MonoBehaviour
{
    public void Save()
    {
        PlayerPrefs.SetString("scene", SceneManager.GetActiveScene().name);

        var savers = FindObjectsOfType<Saver>();
        foreach (var saver in savers)
        {
            saver.Save();
        }
    }
}
