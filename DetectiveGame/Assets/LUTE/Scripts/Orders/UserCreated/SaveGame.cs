using UnityEngine;

[OrderInfo("PlayerPrefs",
              "Save Game",
              "")]
[AddComponentMenu("")]
public class SaveGame : Order
{
    private SavePrefs m_savePrefs;

    private SavePrefs SavePrefs
    {
        get
        {
            if(m_savePrefs == null) { m_savePrefs = FindAnyObjectByType<SavePrefs>(); }
            return m_savePrefs;
        }
    }

    private void Awake()
    {
        m_savePrefs = FindAnyObjectByType<SavePrefs>();
    }

    public override void OnEnter()
    {
        SavePrefs.Save();
        Continue();
    }
    
    public override string GetSummary() 
    { 
        return "Saves game to file."; 
    }
}