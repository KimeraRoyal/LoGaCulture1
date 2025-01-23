using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePrefs : MonoBehaviour
{
    public Action OnSave;
    
    public void Save()
    {
        Debug.Log("Saving...");
        
        PlayerPrefs.SetString("scene", SceneManager.GetActiveScene().name);

        OnSave?.Invoke();
        
        Debug.Log("Saved!");
    }
}
