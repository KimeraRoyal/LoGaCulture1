using UnityEngine;
using UnityEngine.Events;

namespace IP3.Interaction.Click
{
    public class Clickable : MonoBehaviour
    {
        public UnityEvent OnClicked;
        
        public UnityEvent OnHeld;
        public UnityEvent OnReleased;

        private bool m_held;

        public bool Held => m_held;

        public void Click(bool _hold)
        {
            OnClicked?.Invoke();

            if (!_hold || m_held) { return; }
            
            OnHeld?.Invoke();
            m_held = true;
        }

        public void Release()
        {
            if(!m_held) { return; }
            
            OnReleased?.Invoke();
            m_held = false;
        }
    }
}
