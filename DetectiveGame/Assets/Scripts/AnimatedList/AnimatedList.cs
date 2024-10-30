using UnityEngine;

namespace KR
{
    [RequireComponent(typeof(Animator))]
    public class AnimatedList : MonoBehaviour
    {
        private Animator m_animator;

        [SerializeField] private string m_previousParameterName = "Previous";
        private int m_previousHash;

        [SerializeField] private string m_nextParameterName = "Next";
        private int m_nextHash;

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
            m_previousHash = Animator.StringToHash(m_previousParameterName);
            m_nextHash = Animator.StringToHash(m_nextParameterName);
        }

        public void Previous()
            => m_animator.SetTrigger(m_previousHash);

        public void Next()
            => m_animator.SetTrigger(m_nextHash);
    }
}
