using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Flash : MonoBehaviour
{
    private Image m_image;

    [SerializeField] private Color m_flashColor = Color.white;
    
    [SerializeField] private float m_fadeIn = 0.0f;
    [SerializeField] private float m_hold = 1.0f;
    [SerializeField] private float m_fadeOut = 0.0f;
    
    private Sequence m_flashSequence;
    private float m_fade;

    public Sprite Graphic
    {
        get => m_image.sprite;
        set => m_image.sprite = value;
    }

    public Color Color
    {
        get => m_flashColor;
        set => m_flashColor = value;
    }

    public float FadeIn
    {
        get => m_fadeIn;
        set => m_fadeIn = value;
    }

    public float Hold
    {
        get => m_hold;
        set => m_hold = value;
    }

    public float FadeOut
    {
        get => m_fadeOut;
        set => m_fadeOut = value;
    }

    private float Fade
    {
        get => m_fade;
        set
        {
            m_fade = value;
            
            var currentColor = m_flashColor;
            currentColor.a *= m_fade;
            m_image.color = currentColor;
        }
    }

    private void Awake()
    {
        m_image = GetComponent<Image>();
    }

    public void Play()
        => Play(m_hold, m_fadeIn, m_fadeOut);

    public void Play(float _duration)
        => Play(_duration, m_fadeIn, m_fadeOut);

    public void Play(float _duration, float _fadeIn, float _fadeOut)
    {
        if (m_flashSequence is { active: true }) { m_flashSequence.Kill(); }

        m_flashSequence = DOTween.Sequence();
        
        // Fade in, or instantly set to full fade if fade duration is 0
        if (_fadeIn > 0.001f) { m_flashSequence.Append(DOTween.To(() => Fade, x => Fade = x, 1.0f, _fadeIn)); }
        else { Fade = 1.0f; }
        
        // Hold on flash
        if (_duration > 0.001f) { m_flashSequence.AppendInterval(_duration); }
        
        // Fade out, or instantly set to no fade if fade duration is 0
        if (_fadeOut > 0.001f) { m_flashSequence.Append(DOTween.To(() => Fade, x => Fade = x, 0.0f, _fadeOut)); }
        else { m_flashSequence.AppendCallback(() => Fade = 0.0f); }
    }
}
