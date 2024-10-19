using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private Vector3 m_speed = Vector3.up;

    private void Update()
    {
        var eulerAngles = transform.localEulerAngles;
        eulerAngles += m_speed * Time.deltaTime;
        transform.localEulerAngles = eulerAngles;
    }
}
