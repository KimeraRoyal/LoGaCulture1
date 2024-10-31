using System.Collections.Generic;
using System.Linq;
using KW.Flags;
using UnityEngine;

[System.Serializable]
public class NamedFlagCondition
{
    [SerializeField] private NamedFlags m_namedFlags;
    
    [SerializeField] protected bool m_isSet = true;
    [SerializeField] protected string m_flagName;

    [SerializeField] private int m_lastIndex = -1;
    [SerializeField] private int m_lastNamedFlagCount = -1;

    public virtual string FlagName
    {
        get => m_flagName;
#if UNITY_EDITOR
        set => m_flagName = value;
#endif
    }
    
    public virtual bool IsSet
    {
        get => m_isSet;
#if UNITY_EDITOR
        set => m_isSet = value;
#endif
    }

    public NamedFlagCondition() { }

    public NamedFlagCondition(string _flagName, bool _isSet)
    {
        m_flagName = _flagName;
        m_isSet = _isSet;
    }

    public void Validate(NamedFlags _namedFlags)
    {
        if(!m_namedFlags) { m_namedFlags = _namedFlags; }
        
        m_flagName = m_flagName?.ToLower();
        ValidateFlagName(_namedFlags);
    }

    private void ValidateFlagName(NamedFlags _namedFlags)
    {
        var flagNames = _namedFlags.FlagNames;

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

[OrderInfo("Logic",
              "If Named Flag",
              "If the test expression is true, execute the following order (s)")]
[AddComponentMenu("")]
public class IfNamedFlag : Condition
{
    private NamedFlags m_namedFlags;

    private NamedFlags NamedFlags
    {
        get
        {
            if (!m_namedFlags) { m_namedFlags = FindAnyObjectByType<NamedFlags>(); } 
            return m_namedFlags;
        }
    }
    
    public enum AnyOrAll
    {
        AnyOf_OR,
        AllOf_AND
    }

    [Tooltip("Choosing 'AnyOf' will give a true outcome if at least one condition is true. Opting for 'AllOf' will give a true outcome only when all conditions are true.")]
    [SerializeField] protected AnyOrAll anyOrAllCondition;
    [SerializeField] public List<NamedFlagCondition> conditions = new List<NamedFlagCondition>();

#if UNITY_EDITOR
    // // Called when the script is loaded or a value is changed in the inspector (Called in the editor only).
    public override void OnValidate()
    {
        base.OnValidate();

        conditions ??= new List<NamedFlagCondition>();
        if (conditions.Count == 0)
        {
            conditions.Add(new NamedFlagCondition());
        }

        if(!NamedFlags) { return; }
        foreach (var condition in conditions)
        {
            condition.Validate(NamedFlags);
        }
    }
#endif

    public override bool EvaluateConditions()
    {
        if (conditions == null || conditions.Count == 0) { return false; }
        
        bool resultAny = false, resultAll = true;
        foreach (var condition in conditions)
        {
            var curResult = false;
            if (!string.IsNullOrEmpty(condition.FlagName))
            {
                curResult = NamedFlags.IsFlag(condition.FlagName, condition.IsSet);
            }
            resultAll &= curResult;
            resultAny |= curResult;
        }

        return anyOrAllCondition == AnyOrAll.AnyOf_OR ? resultAny : resultAll;
    }

    protected override bool HasRequiredProperties()
    {
        if (conditions == null || conditions.Count == 0) { return false; }
        return conditions.All(condition => !string.IsNullOrEmpty(condition.FlagName));
    }

    public override string GetSummary()
        => $"Check if Named Flag(s) are set";
}