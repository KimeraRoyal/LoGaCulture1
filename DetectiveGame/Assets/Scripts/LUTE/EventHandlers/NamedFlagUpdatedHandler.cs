using KW.Flags;
using UnityEngine;

[EventHandlerInfo("Flag",
    "Named Flag Updated",
    "Executes when the specified named flag changes")]
public class NamedFlagUpdatedHandler : EventHandler
{
    private NamedFlags m_flags;

    [Tooltip("The name of the flag to track")]
    [SerializeField] protected string m_flagName;

    [Tooltip("The mode determining what changes are tracked")]
    [SerializeField] protected FlagUpdatedHandler.Mode m_mode;

    protected virtual void Awake()
    {
        m_flags = FindAnyObjectByType<NamedFlags>();
        m_flags.OnFlagUpdated += OnFlagUpdated;
    }

    private void OnFlagUpdated(string _name, bool _value)
    {
        if  (_name != m_flagName ||
             (m_mode == FlagUpdatedHandler.Mode.ExecuteOnSet && !_value) ||
             (m_mode == FlagUpdatedHandler.Mode.ExecuteOnClear && _value))
        {
            return;
        }

        ExecuteNode();
    }

    public override string GetSummary()
        => $"This node will execute when flag {m_flagName} {FlagUpdatedHandler.GetModeText(m_mode)}.";
}