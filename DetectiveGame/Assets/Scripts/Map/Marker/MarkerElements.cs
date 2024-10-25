using System;
using KR.Elements.Camera;
using KR.Elements.Color;
using KR.Elements.Graphic;
using KR.Elements.Label;
using UnityEngine;

namespace KR.Map.Marker
{
    [RequireComponent(typeof(Marker))]
    public class MarkerElements : MonoBehaviour
    {
        private Marker m_marker;
        
        private ILabelledElement[] m_namedElements;
        
        private IGraphicElement[] m_graphicElements;
        private IColoredElement[] m_coloredElements;
        
        private ICameraElement[] m_cameraElements;

        private void Awake()
        {
            m_marker = GetComponent<Marker>();
            
            m_namedElements = GetComponentsInChildren<ILabelledElement>();
            m_graphicElements = GetComponentsInChildren<IGraphicElement>();
            m_coloredElements = GetComponentsInChildren<IColoredElement>();
            m_cameraElements = GetComponentsInChildren<ICameraElement>();
        }

        private void Start()
        {
            UpdateNamedElements(m_marker.Name);
            UpdateSpriteElements(m_marker.Icon);
            UpdateColoredElements(m_marker.Color);
            UpdateCameraElements(m_marker.TargetCamera);
        }

        private void OnEnable()
        {
            m_marker.OnNameUpdated += UpdateNamedElements;
            m_marker.OnIconUpdated += UpdateSpriteElements;
            m_marker.OnColorUpdated += UpdateColoredElements;
            m_marker.OnTargetCameraUpdated += UpdateCameraElements;
        }

        private void OnDisable()
        {
            m_marker.OnNameUpdated -= UpdateNamedElements;
            m_marker.OnIconUpdated -= UpdateSpriteElements;
            m_marker.OnColorUpdated -= UpdateColoredElements;
            m_marker.OnTargetCameraUpdated -= UpdateCameraElements;
        }

        private void UpdateNamedElements(string _name)
        {
            foreach (var element in m_namedElements)
            {
                element.SetLabel(_name);
            }
        }

        private void UpdateSpriteElements(Sprite _sprite)
        {
            foreach (var element in m_graphicElements)
            {
                element.SetSprite(_sprite);
            }
        }

        private void UpdateColoredElements(Color _color)
        {
            foreach (var element in m_coloredElements)
            {
                element.SetColor(_color);
            }
        }

        private void UpdateCameraElements(Camera _camera)
        {
            foreach (var element in m_cameraElements)
            {
                element.SetCamera(_camera);
            }
        }
    }
}