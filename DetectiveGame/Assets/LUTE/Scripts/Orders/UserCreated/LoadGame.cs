using System;
using UnityEngine;

[OrderInfo("PlayerPrefs",
              "Load Game",
              "")]
[AddComponentMenu("")]
public class LoadGame : Order
{
    private LoadPrefs m_loadPrefs;

    private LoadPrefs LoadPrefs
    {
        get
        {
            if(m_loadPrefs == null) { m_loadPrefs = FindAnyObjectByType<LoadPrefs>(); }
            return m_loadPrefs;
        }
    }

    private void Awake()
    {
        m_loadPrefs = FindAnyObjectByType<LoadPrefs>();
    }

    public override void OnEnter()
    {
        LoadPrefs.Load();
    }
    
    public override string GetSummary() 
    { 
        return "Loads game from file."; 
    }
}