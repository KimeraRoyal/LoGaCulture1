using LUTE.EventHandlers;
using UnityEngine;

[EventHandlerInfo("UI",
    "Button Pressed",
    "Executes when the specified button is pressed")]
public class ButtonPressedHandler : EventHandler
{
    private HandledButtons m_buttons;
    
    [Tooltip("The name of the button to track")]
    [SerializeField] protected string m_buttonName;

    protected virtual void Awake()
    {
        m_buttons = FindAnyObjectByType<HandledButtons>();
        m_buttons.OnButtonPressed += OnButtonPressed;
    }

    private void OnButtonPressed(string _name)
    {
        if(m_buttonName != _name) { return; }
        ExecuteNode();
    }

    public override string GetSummary()
        => $"This node will execute when a button with the name \"{m_buttonName}\" is clicked.";
}
