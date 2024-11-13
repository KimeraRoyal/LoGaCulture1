using UnityEngine;
using UnityEngine.UI;

public class RawImageScroll : MonoBehaviour
{
    private RawImage m_rawImage;

    [SerializeField] private Vector2 m_speed = Vector2.right;

    private Vector2 m_scroll;
    
    private void Awake()
    {
        m_rawImage = GetComponent<RawImage>();
    }

    private void Update()
    {
        m_scroll += m_speed * Time.deltaTime;
        for (var axis = 0; axis < 2; axis++)
        {
            if (m_scroll[axis] < -1.0f) { m_scroll[axis] += 2.0f; }
            if (m_scroll[axis] > 1.0f) { m_scroll[axis] -= 2.0f; }
        }

        var uvRect = m_rawImage.uvRect;
        uvRect.x = m_scroll.x;
        uvRect.y = m_scroll.y;
        m_rawImage.uvRect = uvRect;
    }
}
