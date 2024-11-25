using System;
using KR;
using UnityEngine;

namespace Popups
{
    [RequireComponent(typeof(Openable))]
    public class Popup : MonoBehaviour
    {
        private Openable m_openable;
        
        private PopupLabel m_titleLabel;
        private PopupGraphic m_graphic;
        private PopupLabel m_descriptionLabel;

        public Action OnOpened
        {
            get => m_openable.OnOpened;
            set => m_openable.OnOpened = value;
        }
        public Action OnClosed
        {
            get => m_openable.OnClosed;
            set => m_openable.OnClosed = value;
        }

        private void Awake()
        {
            m_openable = GetComponent<Openable>();
            
            var labels = GetComponentsInChildren<PopupLabel>();
            m_titleLabel = labels[0];
            m_descriptionLabel = labels[1];

            m_graphic = GetComponentInChildren<PopupGraphic>();
        }

        public void Show(string _title = null, Sprite _graphic = null, string _description = null)
        {
            m_titleLabel.Label = _title;
            m_graphic.Graphic = _graphic;
            m_descriptionLabel.Label = _description;
            
            m_openable.Open();
        }
    }
}
