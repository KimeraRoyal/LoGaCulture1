using System;
using UnityEngine;

[OrderInfo("PlayerPrefs",
              "Load Game",
              "")]
[AddComponentMenu("")]
public class LoadGame : Order
{
    private LoadPrefs m_loadPrefs;

    private void Awake()
    {
        m_loadPrefs = FindAnyObjectByType<LoadPrefs>();
    }

    public override void OnEnter()
    {
        m_loadPrefs.Load();
    }
    
    public override string GetSummary() 
    { 
        return "Loads game from file."; 
    }
}