using System;
using UnityEngine;
using UnityEngine.UI;

namespace KR.Map.Marker
{
    public class Marker : MonoBehaviour
    {
        private Button m_button;
        
        private string m_label;

        private Sprite m_icon;
        private Color m_color;

        private Camera m_targetCamera;

        public string Name
        {
            get => m_label;
            set
            {
                if(m_label == value) { return; }
                m_label = value;
                OnNameUpdated?.Invoke(m_label);
            }
        }

        public Color Color
        {
            get => m_color;
            set
            {
                if(m_color == value) { return; }
                m_color = value;
                OnColorUpdated?.Invoke(m_color);
            }
        }

        public Sprite Icon
        {
            get => m_icon;
            set
            {
                if(m_icon == value) { return; }
                m_icon = value;
                OnIconUpdated?.Invoke(m_icon);
            }
        }

        public Camera TargetCamera
        {
            get => m_targetCamera;
            set
            {
                if(m_targetCamera == value) { return; }
                m_targetCamera = value;
                OnTargetCameraUpdated?.Invoke(m_targetCamera);
            }
        }

        public Action<string> OnNameUpdated;
        
        public Action<Sprite> OnIconUpdated;
        public Action<Color> OnColorUpdated;

        public Action<Camera> OnTargetCameraUpdated;

        public Action OnPressed;

        private void Awake()
        {
            m_button = GetComponentInChildren<Button>();
            m_button.onClick.AddListener(Press);
        }

        private void Press()
            => OnPressed?.Invoke();
    }
}
