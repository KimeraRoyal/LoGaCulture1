using KR.TopRow;
using UnityEngine;

[OrderInfo("Menu",
              "Show Top Row Menu",
              "Shows / hides the top row menu, without showing the back button.")]
[AddComponentMenu("")]
public class ShowTopRowMenu : Order
{
    private TopRow m_topRow;

    [SerializeField] private bool m_shown = true;

    private void Awake()
    {
        m_topRow = FindAnyObjectByType<TopRow>();
    }

    public override void OnEnter()
    {
        m_topRow.ShowTopRow(m_shown);
        Continue();
    }

    public override string GetSummary()
        => $"{(m_shown ? "Shows" : "Hides")} the top row of UI.";
}