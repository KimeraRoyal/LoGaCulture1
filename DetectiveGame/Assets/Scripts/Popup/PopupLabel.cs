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
                m_element.SetLabel(value);
                gameObject.SetActive(!string.IsNullOrEmpty(value));
            }
        }
        
        private void Awake()
        {
            m_canvasGroup = GetComponent<CanvasGroup>();
            
            m_element = GetComponentInChildren<ILabelledElement>();
        }
    }
}