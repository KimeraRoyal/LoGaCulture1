using System;
using KW.Flags;
using UnityEngine;

[EventHandlerInfo("Flag",
    "Flag Updated",
    "Executes when the specified flag changes")]
public class FlagUpdatedHandler : EventHandler
{
    public enum Mode
    {
        ExecuteOnSet,
        ExecuteOnClear,
        ExecuteOnChange
    }
    
    private Flags m_flags;

    [Tooltip("The index of the flag to track")]
    [SerializeField] protected int m_flagIndex;

    [Tooltip("The mode determining what changes are tracked")]
    [SerializeField] protected Mode m_mode;

    protected virtual void Awake()
    {
        m_flags = FindAnyObjectByType<Flags>();
        m_flags.OnFlagUpdated += OnFlagUpdated;
    }

    private void OnFlagUpdated(int _index, bool _value)
    {
        if  (_index != m_flagIndex ||
            (m_mode == Mode.ExecuteOnSet && !_value) ||
            (m_mode == Mode.ExecuteOnClear && _value))
        {
            return;
        }

        ExecuteNode();
    }

    public override string GetSummary()
        => $"This node will execute when flag {m_flagIndex} {GetModeText(m_mode)}.";
    
    public static string GetModeText(Mode _mode)
        => _mode switch
        {
            Mode.ExecuteOnSet => "is set",
            Mode.ExecuteOnClear => "is cleared",
            Mode.ExecuteOnChange => "changes",
            _ => throw new ArgumentOutOfRangeException()
        };
}