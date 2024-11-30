using UnityEngine;

[RequireComponent(typeof(RectTransform))] [ExecuteInEditMode]
public class CopySize : MonoBehaviour
{
    private RectTransform m_rectTransform;
    
    [SerializeField] private RectTransform m_target;

    [SerializeField] private bool m_copyWidth = true, m_copyHeight = true;
    
    [SerializeField] private Vector2 m_scale = Vector2.one;
    [SerializeField] private Vector2 m_padding;

    private Vector2 m_lastSize;
    
    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if(!m_target || (m_target.sizeDelta - m_lastSize).magnitude < 0.001f) { return; }
        
        var size = m_rectTransform.sizeDelta;
        if (m_copyWidth)
        {
            size.x = m_target.sizeDelta.x * m_scale.x + m_padding.x;
        }
        if (m_copyHeight)
        {
            size.y = m_target.sizeDelta.y * m_scale.y + m_padding.y;
        }
        m_rectTransform.sizeDelta = size;

        m_lastSize = m_target.sizeDelta;
    }
}
