using System.Collections.Generic;
using UnityEngine;

[OrderInfo("Narrative",
             "Choice",
             "Displays a button in a multiple choice menu to move to next node")]
[AddComponentMenu("")]
public class Choice : Order
{
    [Tooltip("Text to display on the menu button")]
    [TextArea()]
    [SerializeField] protected string text = "Option Text";
    [Tooltip("Notes about the option text for other authors, localization, etc.")]
    [SerializeField] protected string description = "";
    [Tooltip("Node to execute when this option is selected")]
    [SerializeField] public Node targetNode;
    [Tooltip("Hide this option if the target node has been executed previously")]
    [SerializeField] protected bool hideIfVisited;
    [Tooltip("If false, the menu option will be displayed but will not be selectable")]
    [SerializeField] protected bool interactable = true;
    [Tooltip("A custom Menu display to use to display this menu")]
    [SerializeField] protected MenuDialogue setMenuDialogue;
    [Tooltip("If true, this option will be passed to the Menu Dialogue but marked as hidden, this can be used to hide options while maintaining a Menu Shuffle.")]
    [SerializeField] protected bool hideThisOption = false;
    [Tooltip("If true, the menu will close when this option is selected")]
    [SerializeField] protected bool closeMenuOnSelect = true;
    [Tooltip("If true, this option will be passed to the Menu Dialogue but marked as a choice, this can be used to hide options while maintaining a Menu Shuffle.")] //to be implemented

    protected bool isPopupChoice = false;

    public MenuDialogue SetMenuDialogue { get { return setMenuDialogue; } set { setMenuDialogue = value; } }

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
                Debug.Log("menu dialogue set");
                MenuDialogue.ActiveMenuDialogue = setMenuDialogue;
            }

            bool hideOption = (hideIfVisited && targetNode != null && targetNode.GetExecutionCount() > 0) || hideThisOption;

            var menu = MenuDialogue.GetMenuDialogue();
            if (menu != null)
            {
                menu.SetActive(true);

                menu.AddOption(text, interactable, hideOption, targetNode);
            }

            Continue();
        }
    }

    public virtual void SetMenuChoice(Popup popup)
    {
        if (popup != null)
        {
            popup.AddOption(text, interactable, hideIfVisited, targetNode, closeMenuOnSelect);
        }
    }

    public override void GetConnectedNodes(ref List<Node> connectedNodes)
    {
        if (targetNode != null)
        {
            connectedNodes.Add(targetNode);
        }
    }

    public override string GetSummary()
    {
        if (targetNode == null)
        {
            return "Error: No target node selected";
        }

        if (text == "")
        {
            return "Error: No button text selected";
        }

        return text + " : " + targetNode._NodeName;
    }

    public override Color GetButtonColour()
    {
        return new Color32(224, 96, 22, 255);
    }

}
