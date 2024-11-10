using KR.Elements.Graphic;
using UnityEngine;

public class DialogueGraphic : MonoBehaviour
{
    private IGraphicElement[] m_iconElements;

    private RectTransform m_rectTransform;
    private CanvasGroup m_canvasGroup;

    [SerializeField] private RectTransform m_paperTransform;
    
    [SerializeField] private Sprite m_icon;
    [SerializeField] private bool m_shown;

    private Vector2 m_iconSize;
    private Vector2 m_borderSize;

    public Sprite Icon
    {
        get => m_icon;
        set
        {
            if(m_icon == value) { return; }
            m_icon = value;
            UpdateIcons(m_icon);
        }
    }

    public Vector2 Size
    {
        get => m_iconSize;
        set
        {
            if((m_iconSize - value).magnitude < 0.001f) { return; }
            m_iconSize = value;
            UpdateSize(m_iconSize);
        }
    }
    
    public bool Shown
    {
        get => m_shown;
        set
        {
            if (m_shown == value) { return; }
            m_shown = value;
            UpdateShown(m_shown);
        }
    }

    private void Awake()
    {
        m_iconElements = GetComponentsInChildren<IGraphicElement>();

        m_rectTransform = GetComponent<RectTransform>();
        m_canvasGroup = GetComponent<CanvasGroup>();

        m_borderSize = -m_paperTransform.sizeDelta;
        m_iconSize = m_rectTransform.sizeDelta - m_borderSize;
        
        UpdateShown(m_shown);
    }
    
    private void UpdateIcons(Sprite _icon)
    {
        foreach (var iconElement in m_iconElements)
        {
            iconElement.SetSprite(_icon);
        }
    }

    private void UpdateSize(Vector2 _size)
    {
        m_rectTransform.sizeDelta = m_iconSize + m_borderSize;
    }

    private void UpdateShown(bool _shown)
    {
        m_canvasGroup.alpha = m_shown ? 1.0f : 0.0f;
        m_canvasGroup.blocksRaycasts = m_shown;
    }
}