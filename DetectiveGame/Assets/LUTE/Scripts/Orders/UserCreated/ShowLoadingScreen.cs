using System;
using KR;
using UnityEngine;

[OrderInfo("VFX",
              "Show Loading Screen",
              "Shows / closes the loading screen graphic")]
[AddComponentMenu("")]
public class ShowLoadingScreen : Order
{
    private LoadingScreen m_loadingScreen;
    
    [SerializeField] private bool m_show = true;
    [SerializeField] private bool m_waitToAnimate = true;

    private void Awake()
    {
        m_loadingScreen = FindAnyObjectByType<LoadingScreen>();
    }

    public override void OnEnter()
    {
        if(!m_loadingScreen) { Continue(); }
        
        if (m_show)
        {
            m_loadingScreen.OnOpened += OnAnimationComplete;
            m_loadingScreen.Open();
        }
        else
        {
            m_loadingScreen.OnClosed += OnAnimationComplete;
            m_loadingScreen.Close();
        }
        
        if(!m_waitToAnimate) { OnAnimationComplete(); }
    }

    private void OnAnimationComplete()
    {
        if (m_show)
        {
            m_loadingScreen.OnOpened -= OnAnimationComplete;
        }
        else
        {
            m_loadingScreen.OnClosed -= OnAnimationComplete;
        }
        
        Continue();
    }
    
    public override string GetSummary() 
        => $"{(m_show ? "Shows" : "Hides")} the loading screen graphic{(m_waitToAnimate ? $", then waits until it has finished animating {(m_show ? "open" : "closed")}" : "")}.";
}