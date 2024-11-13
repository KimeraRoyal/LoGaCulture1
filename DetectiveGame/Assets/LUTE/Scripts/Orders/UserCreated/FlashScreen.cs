using System;
using UnityEngine;

[OrderInfo("VFX",
              "Flash Screen",
              "Flashes the screen with a colour / image")]
[AddComponentMenu("")]
public class FlashScreen : Order
{
    private Flash m_flash;

    [SerializeField] private bool m_playFlash = true;
    [SerializeField] private VFXConfigurationMode m_configurationMode;
    
    [SerializeField] private Sprite m_graphic;
    [SerializeField] private Color m_color = Color.white;
    
    [SerializeField] private float m_fadeIn = 0.0f;
    [SerializeField] private float m_hold = 1.0f;
    [SerializeField] private float m_fadeOut = 0.0f;
    
    private void Awake()
    {
        m_flash = FindAnyObjectByType<Flash>();
    }

    public override void OnEnter()
    {
        if (m_configurationMode == VFXConfigurationMode.UseSpecified)
        {
            m_flash.Play(m_hold, m_fadeIn, m_fadeOut);
        }
        else
        {
            if (m_configurationMode == VFXConfigurationMode.ChangeSettings)
            {
                m_flash.Graphic = m_graphic;
                m_flash.Color = m_color;

                m_flash.FadeIn = m_fadeIn;
                m_flash.Hold = m_hold;
                m_flash.FadeOut = m_fadeOut;
            }
            m_flash.Play();
        }
        
        Continue();
    }
    
    public override string GetSummary() 
    {
        var doesFadeIn = m_fadeIn > 0.001f; 
        var doesFadeOut = m_fadeOut > 0.001f; 
        var doesFade = doesFadeIn || doesFadeOut;
        var fadeText = doesFadeIn && doesFadeOut ? "in/out" : doesFadeIn ? "in" : "out";
        
        var flashText = $"{(doesFade ? $"Fades {fadeText}" : "Shows")} a flash on screen.";
        var configText = "";
        if (m_playFlash)
        {
            configText = m_configurationMode switch
            {
                VFXConfigurationMode.UseDefault => "Uses default flash settings.",
                VFXConfigurationMode.UseSpecified => "Uses specified flash settings, just once.",
                VFXConfigurationMode.ChangeSettings => "Saves and uses specified flash settings.",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        else
        {
            configText = m_configurationMode == VFXConfigurationMode.ChangeSettings
                ? "Saves specified flash settings."
                : "ERROR: Neither plays flash nor changes settings.";
        }
        return $"{(m_playFlash ? flashText + " " : "")}{configText}";
    }
}