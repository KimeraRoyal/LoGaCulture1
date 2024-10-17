using System.Collections.Generic;
using System.Linq;
using KW.Flags;
using UnityEngine;

[System.Serializable]
public class FlagCondition
{
    [SerializeField] protected bool isSet = true;
    [SerializeField] protected int flagIndex;

    public virtual int FlagIndex => flagIndex;
    public virtual bool IsSet => isSet;

    public FlagCondition() { }

    public FlagCondition(int flagIndex, bool isSet)
    {
        this.flagIndex = flagIndex;
        this.isSet = isSet;
    }
}

[OrderInfo("Logic",
              "If Flag",
              "If the test expression is true, execute the following order (s)")]
[AddComponentMenu("")]
public class IfFlag : Condition
{
    private Flags m_flags;

    private Flags Flags
    {
        get
        {
            if(!m_flags) { m_flags = FindAnyObjectByType<Flags>(); }
            return m_flags;
        }
    }
    
    public enum AnyOrAll
    {
        AnyOf_OR,
        AllOf_AND
    }

    [Tooltip("Choosing 'AnyOf' will give a true outcome if at least one condition is true. Opting for 'AllOf' will give a true outcome only when all conditions are true.")]
    [SerializeField] protected AnyOrAll anyOrAllCondition;
    [SerializeField] public List<FlagCondition> conditions = new List<FlagCondition>();

#if UNITY_EDITOR
    // // Called when the script is loaded or a value is changed in the inspector (Called in the editor only).
    public override void OnValidate()
    {
        base.OnValidate();

        conditions ??= new List<FlagCondition>();
        if (conditions.Count == 0)
        {
            conditions.Add(new FlagCondition());
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
            if (condition.FlagIndex >= 0)
            {
                curResult = Flags.IsFlag(condition.FlagIndex, condition.IsSet);
            }
            resultAll &= curResult;
            resultAny |= curResult;
        }

        return anyOrAllCondition == AnyOrAll.AnyOf_OR ? resultAny : resultAll;
    }

    protected override bool HasRequiredProperties()
    {
        if (conditions == null || conditions.Count == 0) { return false; }
        return conditions.All(condition => condition.FlagIndex >= 0);
    }

    public override string GetSummary()
        => $"Check if Flag(s) are set";
}