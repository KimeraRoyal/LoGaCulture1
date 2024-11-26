using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AspectRatioFitter), typeof(Image))] [ExecuteInEditMode]
public class ImageAspectRatio : MonoBehaviour
{
    private AspectRatioFitter m_fitter;
    private Image m_image;

    private Vector2 m_lastSize;
    
    private void Awake()
    {
        m_fitter = GetComponent<AspectRatioFitter>();
        m_image = GetComponent<Image>();
    }

    private void Start()
    {
        UpdateRatio(m_image.sprite);
    }

    private void Update()
    {
        UpdateRatio(m_image.sprite);
    }

    private void UpdateRatio(Sprite _sprite)
        => UpdateRatio(new Vector2(_sprite.texture.width, _sprite.texture.height));

    private void UpdateRatio(Vector2 _size)
    {
        if((m_lastSize.normalized - _size.normalized).sqrMagnitude < 0.001) { return; }
        m_fitter.aspectRatio = _size.y > 0.001f ? 1.0f / _size.y * _size.x : 0.0f;
        m_lastSize = _size;
    }
}
