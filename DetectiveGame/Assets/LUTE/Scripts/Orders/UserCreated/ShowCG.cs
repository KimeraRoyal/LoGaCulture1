using KR;
using UnityEngine;

[OrderInfo("VFX",
              "Show CG",
              "Shows / hides the currently set CG behind dialogue")]
[AddComponentMenu("")]
public class ShowCG : Order
{
    private CG m_cg;

    [SerializeField] private bool m_shown = true;

    private void Awake()
    {
        m_cg = FindAnyObjectByType<CG>();
    }

    public override void OnEnter()
    {
        m_cg.Shown = m_shown;
        Continue(); 
    }

    public override string GetSummary()
        => $"{(m_shown ? "Shows" : "Hides")} the currently set CG graphic.";
}