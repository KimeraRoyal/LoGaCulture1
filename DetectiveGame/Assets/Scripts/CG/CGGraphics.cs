using KR.Elements.Graphic;
using UnityEngine;

namespace KR
{
    public class CGGraphics : MonoBehaviour
    {
        private CG m_cg;
        
        private IGraphicElement[] m_graphicElements;

        private void Awake()
        {
            m_cg = GetComponentInParent<CG>();
            
            m_graphicElements = GetComponentsInChildren<IGraphicElement>();
            
            m_cg.OnGraphicChanged += OnGraphicChanged;
        }

        private void Start()
        {
            OnGraphicChanged(m_cg.Graphic);
        }

        private void OnGraphicChanged(Sprite _graphic)
        {
            if(!_graphic) { return; }

            foreach (var element in m_graphicElements)
            {
                element.SetSprite(_graphic);
            }
        }
    }
}
