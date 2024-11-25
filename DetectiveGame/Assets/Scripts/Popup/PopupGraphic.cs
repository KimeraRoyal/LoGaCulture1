using KR.Elements.Graphic;
using UnityEngine;

namespace Popups
{
    public class PopupGraphic : MonoBehaviour
    {
        private CanvasGroup m_canvasGroup;
        
        private IGraphicElement m_element;

        public Sprite Graphic
        {
            get => m_element.GetSprite();
            set
            {
                m_canvasGroup.alpha = value != null ? 1.0f : 0.0f;
                m_element.SetSprite(value);
            }
        }
        
        private void Awake()
        {
            m_canvasGroup = GetComponent<CanvasGroup>();
            
            m_element = GetComponentInChildren<IGraphicElement>();
        }
    }
}