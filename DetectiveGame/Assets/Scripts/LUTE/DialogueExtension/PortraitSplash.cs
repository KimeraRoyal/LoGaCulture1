using KR.Elements.Color;
using KR.Elements.Graphic;
using UnityEngine;

public class PortraitSplash : MonoBehaviour
{
    private static readonly int s_splash = Animator.StringToHash("Splash");
    
    private Animator m_animator;

    private IGraphicElement[] m_graphicElements;
    private IColoredElement[] m_coloredElements;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();

        m_graphicElements = GetComponentsInChildren<IGraphicElement>();
        m_coloredElements = GetComponentsInChildren<IColoredElement>();
    }

    public void Play(Sprite _portrait, Color _color)
    {
        SetPortrait(_portrait);
        SetBackgroundColor(_color);
        
        m_animator.SetTrigger(s_splash);
    }

    private void SetPortrait(Sprite _portrait)
    {
        foreach (var graphicElement in m_graphicElements)
        {
            graphicElement.SetSprite(_portrait);
        }
    }

    private void SetBackgroundColor(Color _color)
    {
        foreach (var coloredElement in m_coloredElements)
        {
            coloredElement.SetColor(_color);
        }
    }
}
