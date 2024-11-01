using UnityEngine;
using UnityEngine.EventSystems;

namespace IP3.Interaction.Click
{
    public class BlockClickerOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Clicker m_clicker;

        private void Awake()
        {
            m_clicker = FindObjectOfType<Clicker>();
        }

        public void OnPointerEnter(PointerEventData eventData)
            => m_clicker.Block();

        public void OnPointerExit(PointerEventData eventData)
            => m_clicker.Unblock();
    }
}
