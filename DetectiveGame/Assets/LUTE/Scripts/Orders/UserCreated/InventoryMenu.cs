using MoreMountains.InventoryEngine;
using UnityEngine;
using UnityEngine.Serialization;

[OrderInfo("Inventory",
              "InventoryMenu",
              "Creates a button which will toggle the inventory on/off (rather than using a nested button in popups)")]
[AddComponentMenu("")]
public class InventoryMenu : Order
{
    [Tooltip("Custom icon to display for this menu")]
    [SerializeField] protected Sprite customButtonIcon;
    [FormerlySerializedAs("setIconButton")]
    [Tooltip("A custom popup class to use to display this menu - if one is in the scene it will be used instead")]
    [SerializeField] protected PopupIcons m_setIconsButton;
    [Tooltip("The inventory to toggle")]
    [SerializeField] protected Inventory inventory;
    [Tooltip("If true, the popup icon will be displayed, otherwise it will be hidden")]
    [SerializeField] protected bool showIcon = true;

    public override void OnEnter()
    {
        if (inventory == null)
        {
            inventory = Inventory.FindInventory("MainInventory", "Player1");
            if(inventory == null)
            {
                Continue();
                return;
            }
        }

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

        UnityEngine.Events.UnityAction action = () =>
        {
        InventoryInputManager inventoryInputManager = inventory.GetComponentInChildren<InventoryInputManager>();
        if (inventoryInputManager != null)
        {
        inventoryInputManager.ToggleInventory();
        }
        };
        popupIcon.SetAction(action);
        popupIcon.MoveToNextOption();

        Continue();
    }

        public override string GetSummary()
        {
        return "Creates a button which will toggle the inventory on/off (rather than using a nested button in popups)";
        }
}