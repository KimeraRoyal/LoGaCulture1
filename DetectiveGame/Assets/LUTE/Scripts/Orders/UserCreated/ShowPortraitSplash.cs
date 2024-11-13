using UnityEngine;

[OrderInfo("Narrative",
              "Show Portrait Splash",
              "Shows a splash animation of a character portrait")]
[AddComponentMenu("")]
public class ShowPortraitSplash : Order
{
    private PortraitSplash m_portraitSplash;

    [SerializeField] protected Sprite m_portrait;
    [SerializeField] protected Color m_color = Color.white;
    
    private void Awake()
    {
        m_portraitSplash = FindAnyObjectByType<PortraitSplash>();
    }

    public override void OnEnter() 
    {
        m_portraitSplash.Play(m_portrait, m_color);
        Continue();
    }

    public override string GetSummary()
        => "Shows a splash animation of a character portrait.";
}