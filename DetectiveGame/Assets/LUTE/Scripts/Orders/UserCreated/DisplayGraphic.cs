using UnityEngine;

[OrderInfo("VFX",
              "Display Graphic",
              "Pops up an image in front of the screen")]
[AddComponentMenu("")]
public class DisplayGraphic : Order
{
    private DialogueGraphic m_dialogueGraphic;

    [SerializeField] protected bool m_show = true;

    [SerializeField] protected Sprite m_icon;
    [SerializeField] protected Vector2 m_iconSize = new Vector2(400, 400);

    private void Awake()
    {
        m_dialogueGraphic = FindAnyObjectByType<DialogueGraphic>();
    }

    public override void OnEnter()
    {
        m_dialogueGraphic.Shown = m_show;

        if (m_show)
        {
            m_dialogueGraphic.Icon = m_icon;
            m_dialogueGraphic.Size = m_iconSize;
        }

        Continue();
    }

    public override string GetSummary()
        => $"{(m_show ? "Shows" : "Hides")} an icon in front of dialogue.";
}