using UnityEngine;

[OrderInfo("VFX",
              "Set Animator Boolean",
              "Sets a boolean property of an Animator to the given value")]
[AddComponentMenu("")]
public class SetAnimatorBoolean : Order
{
    [SerializeField] private Animator m_animator;
    
    [SerializeField] private string m_key;
    [SerializeField] private bool m_value;

    private int m_keyHash;

    private void Awake()
    {
        m_keyHash = Animator.StringToHash(m_key);
    }

    public override void OnEnter()
    {
        m_animator.SetBool(m_keyHash, m_value);
        Continue();
    }

    public override string GetSummary()
        => m_animator ? $"Sets the value of animator property \"{m_key}\" to {m_value}" : "ERROR: No animator set.";
}