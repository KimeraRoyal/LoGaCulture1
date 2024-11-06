using System.Collections;
using IP3.Interaction.Click;
using UnityEngine;

namespace IP3
{
    public class DestroyOnClicked : MonoBehaviour
    {
        private Clickable m_clickable;

        [SerializeField] private float m_waitDuration = 1.0f;

        private bool m_primed;

        private void Awake()
        {
            m_clickable = GetComponentInChildren<Clickable>();
            m_clickable.OnClicked.AddListener(OnClicked);
        }

        private void OnClicked()
        {
            if(m_primed) { return; }
            m_primed = true;

            if (m_waitDuration < 0.001f)
            {
                DestroySelf();
                return;
            }

            StartCoroutine(DestructionCountdown());
        }

        private IEnumerator DestructionCountdown()
        {
            yield return new WaitForSeconds(m_waitDuration);
            DestroySelf();
        }

        private void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}
