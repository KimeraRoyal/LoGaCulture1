using System;
using KW.Flags;
using UnityEngine;

[OrderInfo("Flags",
              "Set Named Flag",
              "Sets or clears a named story flag")]
[AddComponentMenu("")]
public class SetNamedFlag : Order
{
    private NamedFlags m_namedFlags;
    
    [SerializeField] protected string m_flagName; 
    [SerializeField] protected Flags.Operation m_operation;

    [SerializeField] private int m_lastIndex = -1;
    [SerializeField] private int m_lastNamedFlagCount = -1;

    private NamedFlags NamedFlags
    {
        get
        {
            if(!m_namedFlags) { m_namedFlags = FindAnyObjectByType<NamedFlags>(); }
            return m_namedFlags;
        }
    }

    private void Awake()
    {
        ValidateFlagName();
    }

    public override void OnEnter()
    {
        switch (m_operation)
        {
            case Flags.Operation.Set:
                NamedFlags.SetFlag(m_flagName);
                break;
            case Flags.Operation.Clear:
                NamedFlags.ClearFlag(m_flagName);
                break;
            case Flags.Operation.Toggle:
                NamedFlags.ToggleFlag(m_flagName);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        Continue();
    }

    public override string GetSummary()
    {
        ValidateFlagName();
        return $"{m_operation}s Named Flag \"{m_flagName}\"";
    }

#if UNITY_EDITOR
    public override void OnValidate()
    {
        m_flagName = m_flagName?.ToLower();
        ValidateFlagName();
        
        base.OnValidate();
    }
#endif

    private void ValidateFlagName()
    {
        if(!NamedFlags) { return; }
        var flagNames = NamedFlags.FlagNames;

        for (var i = 0; i < flagNames.Count; i++)
        {
            if(flagNames[i] != m_flagName) { continue; }
            
            m_lastIndex = i;
            m_lastNamedFlagCount = flagNames.Count;
            
            return;
        }

        var validIndex = m_lastIndex >= 0 && m_lastNamedFlagCount == flagNames.Count;
        m_flagName = validIndex ? flagNames[m_lastIndex] : "";
        if (!validIndex) m_lastIndex = -1;
    }
}