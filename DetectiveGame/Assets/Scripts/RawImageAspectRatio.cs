using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AspectRatioFitter), typeof(RawImage))] [ExecuteInEditMode]
public class RawImageAspectRatio : MonoBehaviour
{
    private AspectRatioFitter m_fitter;
    private RawImage m_image;

    private Vector2 m_lastSize;
    
    private void Awake()
    {
        m_fitter = GetComponent<AspectRatioFitter>();
        m_image = GetComponent<RawImage>();
    }

    private void Start()
    {
        UpdateRatio();
    }

    private void Update()
    {
        UpdateRatio();
    }

    private void UpdateRatio()
    {
        var parentAxis = 0;
        var childAxis = 1;
        switch (m_fitter.aspectMode)
        {
            case AspectRatioFitter.AspectMode.WidthControlsHeight:
                parentAxis = 1;
                childAxis = 0;
                break;
            case AspectRatioFitter.AspectMode.HeightControlsWidth:
                parentAxis = 0;
                childAxis = 1;
                break;
            case AspectRatioFitter.AspectMode.None:
            case AspectRatioFitter.AspectMode.FitInParent:
            case AspectRatioFitter.AspectMode.EnvelopeParent:
            default:
                return;
        }
        UpdateRatio(m_image.texture, parentAxis, childAxis);
    }

    private void UpdateRatio(Texture _texture, int _parentAxis, int _childAxis = 0)
        => UpdateRatio(new Vector2(_texture.width, _texture.height), _parentAxis, _childAxis);

    private void UpdateRatio(Vector2 _size, int _parentAxis = 1, int _childAxis = 0)
    {
        if((m_lastSize.normalized - _size.normalized).sqrMagnitude < 0.001) { return; }
        m_fitter.aspectRatio = _size[_parentAxis] > 0.001f ? 1.0f / _size[_parentAxis] * _size[_childAxis] : 0.0f;
        m_lastSize = _size;
    }
}
