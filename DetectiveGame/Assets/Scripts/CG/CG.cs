using System;
using UnityEngine;

namespace KR
{
    public class CG : MonoBehaviour
    {
        [SerializeField] private Sprite m_graphic;
        [SerializeField] private bool m_shown;

        public Sprite Graphic
        {
            get => m_graphic;
            set
            {
                Shown = value;
                
                if(m_graphic == value) { return; }
                
                m_graphic = value;
                OnGraphicChanged?.Invoke(m_graphic);
            }
        }

        public bool Shown
        {
            get => m_shown;
            set
            {
                if(m_shown == value) { return; }

                m_shown = value;
                if(m_shown) { OnShown?.Invoke(); }
                else { OnHidden?.Invoke(); }
            }
        }

        public Action<Sprite> OnGraphicChanged;
        
        public Action OnShown;
        public Action OnHidden;

        public void Show()
            => Shown = true;

        public void Hide()
            => Shown = false;
    }
}
