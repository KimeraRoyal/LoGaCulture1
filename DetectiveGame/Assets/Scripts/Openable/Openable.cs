using UnityEngine;

namespace KR
{
    [RequireComponent(typeof(Animator))]
    public class Openable : MonoBehaviour
    {
        private Animator m_animator;

        [SerializeField] private string m_openParameterName = "Open";
        private int m_openHash;

        private bool m_opened;

        public bool Opened
        {
            get => m_opened;
            set
            {
                if(m_opened == value) { return; }
                
                m_opened = value;
                m_animator.SetBool(m_openHash, value);
            }
        }

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
            m_openHash = Animator.StringToHash(m_openParameterName);

            m_opened = m_animator.GetBool(m_openHash);
        }

        public void Open()
            => Opened = true;

        public void Close()
            => Opened = false;
    }
}
