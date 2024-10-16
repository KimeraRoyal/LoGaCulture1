using UnityEngine;
using UnityEngine.Events;

[OrderInfo("Character",
              "ProfileMenu",
              "Creates a button that toggles the profile")]
[AddComponentMenu("")]
public class ProfileMenu : Order
{
    [Tooltip("Custom icon to display for this menu")]
    [SerializeField] protected Sprite customButtonIcon;
    
    [Tooltip("A custom popup class to use to display this menu - if one is in the scene it will be used instead")]
    [SerializeField] protected PopupIcon setIconButton;
    
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

        if (setIconButton)
        {
            PopupIcon.ActivePopupIcon = setIconButton;
        }

        var popupIcon = PopupIcon.GetPopupIcon();
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