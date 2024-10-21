using UnityEngine;
using UnityEngine.Serialization;

[OrderInfo("Menu",
             "Popup Menu",
             "Creates a popup menu icon which displays a menu when clicked; the menu is populated by the orders on this node but only supports specific orders")]
[AddComponentMenu("")]
public class PopupMenu : Order
{
    [Tooltip("Custom icon to display for this menu")]
    [SerializeField] protected Sprite customPopupIcon;
    [FormerlySerializedAs("setPopupMenuIcon")]
    [Tooltip("A custom popup class to use to display this menu - if one is in the scene it will be used instead")]
    [SerializeField] protected PopupIcons m_setPopupMenuIcons;
    [Tooltip("A custom Menu display to use to display this popup menu")]
    [SerializeField] protected Popup popupWindow;
    [Tooltip("If true, the popup icon will be displayed, otherwise it will be hidden")]
    [SerializeField] protected bool showIcon = true;

    public Popup SetPopupWindow { get { return popupWindow; } set { popupWindow = value; } }

    public override void OnEnter()
    {
        if (m_setPopupMenuIcons != null)
        {
            PopupIcons.activePopupIcons = m_setPopupMenuIcons;
        }

        if (SetPopupWindow != null)
        {
            Popup.ActivePopupWindow = SetPopupWindow;
        }

        var popupIcon = PopupIcons.GetPopupIcons();
        if (popupIcon != null && showIcon)
        {
            if (customPopupIcon != null)
            {
                popupIcon.SetIcon(customPopupIcon);
            }
            popupIcon.SetActive(true);
        }
        var popupWindow = Popup.GetPopupWindow();
        if (popupWindow != null)
        {
            var orders = ParentNode.OrderList;
            popupIcon.SetPopupWindow(popupWindow);
            UnityEngine.Events.UnityAction action = () =>
{
    popupWindow.OpenClose();
};
            popupIcon.SetAction(action);
            if (orders.Count > 0)
            {
                popupWindow.SetOrders(orders);
                popupWindow.CreateMenuGUI();
            }
        }
        popupIcon.MoveToNextOption();

        Continue();
    }
}