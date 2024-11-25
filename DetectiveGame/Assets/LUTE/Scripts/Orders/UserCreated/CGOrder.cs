using KR;
using UnityEngine;

[OrderInfo("VFX",
              "CG",
              "Shows a specified image as a CG behind dialogue. Will cause the CG to appear if the image isn't null, or hide it if the image is null.")]
[AddComponentMenu("")]
public class CGOrder : Order
{
    private CG m_cg;
    
    [SerializeField] private Sprite m_graphic;

    private void Awake()
    {
        m_cg = FindAnyObjectByType<CG>();
    }

    public override void OnEnter()
    {
        m_cg.Graphic = m_graphic;
        Continue(); 
    }

    public override string GetSummary()
        => $"{(m_cg ? "Changes" : "Removes")} the CG graphic. {(m_cg ? "Will cause the CG to appear if it is currently hidden." : "Will cause the CG to disappear if it is currently shown.")}";
}