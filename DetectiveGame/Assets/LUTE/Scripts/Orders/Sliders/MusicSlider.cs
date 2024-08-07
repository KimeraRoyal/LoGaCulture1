using System;
using UnityEngine;
using UnityEngine.EventSystems;

[OrderInfo("Menu", "Music Slider", "Displays a slider in a menu to adjust the music volume")]
public class MusicSlider : OptionSlider
{
    private void Awake()
    {
        if (sliderLabel.Length <= 0)
        {
            sliderLabel = "music volume";
        }

        targetFloat = LogaManager.Instance.SoundManager.GetVolume();

        hideOption = (hideIfMoved && targetFloat < 0) || hideThisOption;
    }
    public override void OnEnter()
    {
        //go through the list of orders to determine if one is a popup 
        //we can then determine if we need to set the menu dialogue or to popup menu
        //if we are a popup choice, we don't need to set the menu dialogue

        var orders = ParentNode.OrderList;
        if (orders.Count > 0)
        {
            foreach (Order order in orders)
            {
                if (order is PopupMenu)
                {
                    isPopupChoice = true;
                }
            }
        }

        if (!isPopupChoice)
        {
            if (setMenuDialogue != null)
            {
                MenuDialogue.ActiveMenuDialogue = setMenuDialogue;
            }

            var menu = MenuDialogue.GetMenuDialogue();
            if (menu != null)
            {
                menu.SetActive(true);

                UnityEngine.Events.UnityAction<float> action = (float value) =>
                {
                    LogaManager.Instance.SoundManager.SetAudioVolume(value, 0, null);
                };

                menu.AddOptionSlider(interactable, targetFloat, hideOption, action, sliderLabel);
            }

            Continue();
        }
    }

    public override void SetSliderOptions(Popup popup)
    {
        if (popup != null)
        {
            UnityEngine.Events.UnityAction<float> action = (float value) =>
            {
                LogaManager.Instance.SoundManager.SetAudioVolume(value, 0, null);
            };

            popup.AddOptionSlider(interactable, targetFloat, hideOption, action, sliderLabel);
        }
    }

    public override string GetSummary()
    {
        if (!string.IsNullOrEmpty(sliderLabel))
        {
            return sliderLabel + ": interactable: " + interactable;
        }
        else
        {
            return interactable.ToString();
        }
    }
}
