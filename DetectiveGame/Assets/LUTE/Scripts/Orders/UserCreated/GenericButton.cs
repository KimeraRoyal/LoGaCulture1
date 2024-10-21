using UnityEngine;
using UnityEngine.Serialization;

[OrderInfo("Menu",
              "GenericButton",
              "Creates a button which allows for generic event inputs")]
[AddComponentMenu("")]
public class GenericButton : Order
{
  [Tooltip("Custom icon to display for this menu")]
  [SerializeField] protected Sprite customButtonIcon;
  [FormerlySerializedAs("setIconButton")]
  [Tooltip("A custom popup class to use to display this menu - if one is in the scene it will be used instead")]
  [SerializeField] protected PopupIcons m_setIconsButton;
  [Tooltip("If true, the popup icon will be displayed, otherwise it will be hidden")]
  [SerializeField] protected bool showIcon = true;
  [Tooltip("The event to call when the button is clicked")]
  [SerializeField] protected UnityEngine.Events.UnityEvent buttonEvent;
  public override void OnEnter()
  {
    if (m_setIconsButton != null)
    {
      PopupIcons.activePopupIcons = m_setIconsButton;
    }

    var popupIcon = PopupIcons.GetPopupIcons();
    if (popupIcon != null)
    {
      if (customButtonIcon != null)
      {
        popupIcon.SetIcon(customButtonIcon);
      }
    }
    if (showIcon)
    {
      popupIcon.SetActive(true);
    }
    popupIcon.SetAction(buttonEvent.Invoke);
    popupIcon.MoveToNextOption();
    Continue();
  }

  public override string GetSummary()
  {
    return "Creates a button which allows for generic event inputs";
  }
}