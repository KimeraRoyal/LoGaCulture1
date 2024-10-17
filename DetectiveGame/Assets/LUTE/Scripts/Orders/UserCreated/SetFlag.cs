using System;
using KW.Flags;
using UnityEngine;

[OrderInfo("Flag",
              "Set Flag",
              "Sets or clears a story flag")]
[AddComponentMenu("")]
public class SetFlag : Order
{
    private Flags m_flags;
    
    [SerializeField] protected int m_flagIndex; 
    [SerializeField] protected Flags.Operation m_operation;

    private void Awake()
    {
        m_flags = FindAnyObjectByType<Flags>();
    }

    public override void OnEnter()
    {
        switch (m_operation)
        {
            case Flags.Operation.Set:
                m_flags.SetFlag(m_flagIndex);
                break;
            case Flags.Operation.Clear:
                m_flags.ClearFlag(m_flagIndex);
                break;
            case Flags.Operation.Toggle:
                m_flags.ToggleFlag(m_flagIndex);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        Continue();
    }

    public override string GetSummary()
        => $"{m_operation}s Flag {m_flagIndex}";
}