using System;
using UnityEngine;
using UnityEngine.UI;

namespace IP1
{
    [RequireComponent(typeof(CanvasScaler))] [ExecuteInEditMode]
    public class ScalePixelCanvas : MonoBehaviour
    {
        private enum Mode
        {
            FitWidth,
            FitHeight
        }
        
        private CanvasScaler m_scaler;

        [SerializeField] private Mode m_mode;
        [SerializeField] private int m_sizeToFit = 100;

        private int m_previousSizeToFit;
        private int m_previousSize;

        private void Awake()
        {
            m_scaler = GetComponent<CanvasScaler>();
        }

        private void Start()
        {
            Scale(GetScreenSize());
        }

        private void Update()
        {
            var size = GetScreenSize();

            if(size == m_previousSize && m_sizeToFit == m_previousSizeToFit) { return; }

            Scale(size);
            
            m_previousSize = size;
            m_previousSizeToFit = m_sizeToFit;
        }

        private void Scale(int _screenSize)
        {
            var factor = (float) _screenSize / m_sizeToFit;
            if (factor >= 1.0f)
            {
                factor = (int)factor;
            }
            m_scaler.scaleFactor = factor;
        }
        
        private int GetScreenSize()
            => m_mode switch
            {
                Mode.FitWidth => Screen.width,
                Mode.FitHeight => Screen.height,
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}
