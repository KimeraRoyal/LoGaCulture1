using UnityEngine;

public class Hover : MonoBehaviour
{
    [SerializeField] private Vector3 m_amount = Vector3.up;
    [SerializeField] private float m_speed = 1.0f;

    private Vector3 m_anchor;

    private void Awake()
    {
        m_anchor = transform.localPosition;
    }

    private void Update()
    {
        transform.localPosition = m_anchor + m_amount * Mathf.Sin(Time.time * m_speed);
    }
}
