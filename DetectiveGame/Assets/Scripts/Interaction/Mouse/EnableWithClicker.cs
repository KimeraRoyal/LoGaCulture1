using System;
using IP3.Interaction.Click;
using UnityEngine;

namespace IP3
{
    public class EnableWithClicker : MonoBehaviour
    {
        private Clicker m_clicker;

        [SerializeField] private MonoBehaviour[] m_behaviours;

        private bool m_enabled;

        private void Awake()
        {
            m_clicker = FindObjectOfType<Clicker>();
        }

        private void Update()
        {
            var notBlocked = !m_clicker.Blocked;
            if(m_enabled == notBlocked) { return; }

            foreach (var behaviour in m_behaviours)
            {
                behaviour.enabled = notBlocked;
            }
            
            m_enabled = notBlocked;
        }
    }
}
