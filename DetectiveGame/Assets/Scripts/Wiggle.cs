using UnityEngine;

public class Wiggle : MonoBehaviour
{
    [SerializeField] private Vector3 m_minRotation = Vector3.zero;
    [SerializeField] private Vector3 m_maxRotation = Vector3.zero;
    
    [SerializeField] private float m_speed = 1.0f;

    private float m_offset;

    private void Awake()
    {
        m_offset = Random.Range(0.0f, Mathf.PI);
    }

    private void Update()
    {
        var t = Mathf.Sin(Time.time * m_speed + m_offset) * 0.5f + 0.5f;
        transform.localEulerAngles = Vector3.Lerp(m_minRotation, m_maxRotation, t);
    }
}
