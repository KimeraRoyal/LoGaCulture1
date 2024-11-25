using KR.Elements.Label;
using UnityEngine;

namespace Popups
{
    public class PopupLabel : MonoBehaviour
    {
        private CanvasGroup m_canvasGroup;
        
        private ILabelledElement m_element;

        public string Label
        {
            get => m_element.GetLabel();
            set
            {
                m_canvasGroup.alpha = value != null ? 1.0f : 0.0f;
                m_element.SetLabel(value);
            }
        }
        
        private void Awake()
        {
            m_canvasGroup = GetComponent<CanvasGroup>();
            
            m_element = GetComponentInChildren<ILabelledElement>();
        }
    }
}