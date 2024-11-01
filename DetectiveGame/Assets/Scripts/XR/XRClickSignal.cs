using IP3.Interaction.Click;
using UnityEngine;

public class XRClickSignal : MonoBehaviour
{
    private XRClickHolder m_clickHolder;
    
    private Clickable m_clickable;

    private void Awake()
    {
        m_clickHolder = FindAnyObjectByType<XRClickHolder>();
        
        m_clickable = GetComponentInChildren<Clickable>();
        m_clickable.OnClicked.AddListener(m_clickHolder.Click);
    }
}
