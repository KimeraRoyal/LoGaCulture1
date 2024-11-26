using UnityEngine;

[OrderInfo("Narrative",
              "Show Popup",
              "Shows a popup which can (optionally) contain a title, graphic, and text description")]
[AddComponentMenu("")]
public class ShowPopup : Order
{
    private Popups.Popup m_popup;

    [SerializeField] private string m_title;
    [SerializeField] private Sprite m_graphic;
    [SerializeField] [TextArea(3, 5)] private string m_description;

    private void Awake()
    {
        m_popup = FindAnyObjectByType<Popups.Popup>();
    }

    public override void OnEnter()
    {
        var title = m_title != "" ? m_title : null;
        var description = m_description != "" ? m_description : null;

        m_popup.OnClosed += OnPopupSelected;
        m_popup.Show(title, m_graphic, description);
    }

    private void OnPopupSelected()
    {
        m_popup.OnClosed -= OnPopupSelected;
        Continue();
    }

    public override string GetSummary()
    {
        var combinedString = "";
        var count = 0;
        
        if (!string.IsNullOrEmpty(m_title))
        {
            combinedString += "title";
            count++;
        }
        if (m_graphic != null)
        {
            if (count > 0)
            {
                combinedString += !string.IsNullOrEmpty(m_description) ? ", " : " and ";
            }
            combinedString += "graphic";
            count++;
        }
        if (!string.IsNullOrEmpty(m_description))
        {
            if (count > 0)
            {
                combinedString += count > 1 ? ", and " : " and ";
            }
            combinedString += "description";
            count++;
        }

        return count > 0 ? $"Shows a popup with a {combinedString}." : "ERROR: No popup elements assigned.";
    } 
}