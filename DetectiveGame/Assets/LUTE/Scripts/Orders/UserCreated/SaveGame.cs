using UnityEngine;

[OrderInfo("PlayerPrefs",
              "Save Game",
              "")]
[AddComponentMenu("")]
public class SaveGame : Order
{
    private SavePrefs m_savePrefs;

    private void Awake()
    {
        m_savePrefs = FindAnyObjectByType<SavePrefs>();
    }

    public override void OnEnter()
    {
        m_savePrefs.Save();
        Continue();
    }
    
    public override string GetSummary() 
    { 
        return "Saves game to file."; 
    }
}