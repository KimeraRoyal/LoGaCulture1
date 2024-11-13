using UnityEngine;

[OrderInfo("VFX",
              "Shake Dialogue Portrait",
              "Shakes the character dialogue portrait")]
[AddComponentMenu("")]
public class ShakeDialoguePortrait : Order
{
    private Shake m_shake;
    
    [SerializeField] private bool m_playShake = true;
    [SerializeField] private VFXConfigurationMode m_configurationMode;
    
    [SerializeField] protected float m_duration = 1.0f;
    [SerializeField] protected Vector3 m_strength = Vector3.one;
    [SerializeField] protected int m_vibrato = 10;
    [SerializeField] protected float m_randomness = 45.0f;

    private void Awake()
    {
        m_shake = FindAnyObjectByType<DialogueBox>().PortraitShake;
    }
    
    public override void OnEnter()
    {
        if (m_configurationMode == VFXConfigurationMode.UseSpecified)
        {
            m_shake.Play(m_duration, m_strength, m_vibrato, m_randomness);
        }
        else
        {
            if (m_configurationMode == VFXConfigurationMode.ChangeSettings)
            {
                m_shake.Duration = m_duration;
                m_shake.Strength = m_strength;
                m_shake.Vibrato = m_vibrato;
                m_shake.Randomness = m_randomness;
            }
            m_shake.Play();
        }
        
        Continue();
    }
    
    public override string GetSummary() 
    { 
        return ""; 
    }
}