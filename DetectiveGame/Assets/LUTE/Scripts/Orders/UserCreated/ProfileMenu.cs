using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[OrderInfo("Character",
              "ProfileMenu",
              "Creates a button that toggles the profile")]
[AddComponentMenu("")]
public class ProfileMenu : Order
{
    [Tooltip("Custom icon to display for this menu")]
    [SerializeField] protected Sprite customButtonIcon;
    
    [FormerlySerializedAs("setIconButton")]
    [Tooltip("A custom popup class to use to display this menu - if one is in the scene it will be used instead")]
    [SerializeField] protected PopupIcons m_setIconsButton;
    
    [Tooltip("If true, the popup icon will be displayed, otherwise it will be hidden")]
    [SerializeField] protected bool showIcon = true;
    
    public override void OnEnter() 
    { 
        CreateIcon();
        Continue();
    }

    private void CreateIcon()
    {
        var engine = GetEngine();
        if (engine == null) { return; }

        var profile = FindAnyObjectByType<Profile>();
        if (profile == null) { return; }

        if (m_setIconsButton)
        {
            PopupIcons.activePopupIcons = m_setIconsButton;
        }

        var popupIcon = PopupIcons.GetPopupIcons();
        if (popupIcon && customButtonIcon)
        {
            popupIcon.SetIcon(customButtonIcon);
        }
        
        if (showIcon)
        {
            popupIcon.SetActive(true);
        }
        
        UnityAction action = profile.Toggle;
        
        popupIcon.SetAction(action);
        popupIcon.MoveToNextOption();
    }

  public override string GetSummary()
    => "Creating a custom icon to open/close the profile (if one exists)";
}