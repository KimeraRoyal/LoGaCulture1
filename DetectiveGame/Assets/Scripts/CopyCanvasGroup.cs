using UnityEngine;

[RequireComponent(typeof(CanvasGroup))] [ExecuteInEditMode]
public class CopyCanvasGroup : MonoBehaviour
{
    private CanvasGroup m_canvasGroup;

    [SerializeField] private CanvasGroup m_target;

    private float m_lastAlpha;
    
    private void Awake()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if(!m_target) { return; }

        UpdateAlpha();
        m_canvasGroup.blocksRaycasts = m_target.blocksRaycasts;
        m_canvasGroup.interactable = m_target.interactable;
    }

    private void UpdateAlpha()
    {
        if(Mathf.Abs(m_target.alpha - m_lastAlpha) < 0.001f) { return; }

        m_canvasGroup.alpha = m_target.alpha;
        m_lastAlpha = m_target.alpha;
    }
}
