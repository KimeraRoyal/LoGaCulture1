using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private Vector3 m_speed = Vector3.up;

    [SerializeField] private bool m_clamp;
    [SerializeField] private Vector3 m_min = Vector3.one * -360.0f, m_max = Vector3.one * 360.0f;

    private Vector3 m_clampRange;

    private void Start()
    {
        m_clampRange = m_max - m_min;
    }

    private void Update()
    {
        var eulerAngles = transform.localEulerAngles;
        eulerAngles += m_speed * Time.deltaTime;
        if (m_clamp)
        {
            for (var i = 0; i < 3; i++)
            {
                if (eulerAngles[i] < m_min[i] - 0.001f) { eulerAngles[i] += m_clampRange[i]; }
                else if (eulerAngles[i] > m_max[i] + 0.001f) { eulerAngles[i] -= m_clampRange[i]; }
            }
        }
        transform.localEulerAngles = eulerAngles;
    }
}
