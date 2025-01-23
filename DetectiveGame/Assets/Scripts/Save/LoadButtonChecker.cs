using Save;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LoadButtonChecker : Saver
{
    private Button m_button;

    private float m_timer;
    
    private void Awake()
    {
        m_button = GetComponent<Button>();
        
        m_button.interactable = PlayerPrefs.HasKey("scene");
    }

    public override void Save()
    {
        m_button.interactable = PlayerPrefs.HasKey("scene"); 
    }

    public override void Load()
    {
        // Nothing needed
    }
}
