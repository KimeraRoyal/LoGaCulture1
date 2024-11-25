using DG.Tweening;
using UnityEngine;

namespace KR
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CGFader : MonoBehaviour
    {
        private CG m_cg;

        private CanvasGroup m_canvasGroup;

        [SerializeField] private float m_showDuration = 1.0f;
        [SerializeField] private float m_hideDuration = 1.0f;

        private Tween m_fadeTween;

        private void Awake()
        {
            m_cg = GetComponentInParent<CG>();

            m_canvasGroup = GetComponent<CanvasGroup>();
            
            m_cg.OnShown += OnShown;
            m_cg.OnHidden += OnHidden;
        }

        private void Start()
            => Fade(m_cg.Shown ? 1.0f : 0.0f, 0.0f);

        private void OnShown()
            => Fade(1.0f, m_showDuration);

        private void OnHidden()
            => Fade(0.0f, m_hideDuration);

        private void Fade(float _target, float _duration)
        {
            if (m_fadeTween is { active: true }) { m_fadeTween.Kill(); }

            if (_duration > 0.001f)
            {
                m_fadeTween = DOTween.To(() => m_canvasGroup.alpha, alpha => m_canvasGroup.alpha = alpha, _target, _duration);
            }
            else
            {
                m_canvasGroup.alpha = _target;
            }
        }
    }
}
