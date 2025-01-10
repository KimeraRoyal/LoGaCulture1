using System;
using UnityEngine;

[OrderInfo("Inventory",
              "Show Item Popups",
              "Toggles whether item popups are shown")]
[AddComponentMenu("")]
public class ShowItemPopups : Order
{
    private ItemPopup m_itemPopup;

    [SerializeField] private bool m_shown = true;

    private void Awake()
    {
        m_itemPopup = FindAnyObjectByType<ItemPopup>();
    }

    public override void OnEnter()
    {
        m_itemPopup.Enabled = m_shown;
        Continue();
    }

    public override string GetSummary()
        => $"Sets item popups to be {(m_shown ? "shown": "hidden")}.";
}