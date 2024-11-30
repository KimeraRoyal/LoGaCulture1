using UnityEngine;

namespace KR
{
    public class FrameAnimation : MonoBehaviour
    {
        private AnimatedComponent[] m_components;

        [SerializeField] private int m_fps = 12;

        private float m_frameDuration;

        private int m_currentFrame;
        private float m_timer;
        
        private void Awake()
        {
            m_components = GetComponentsInChildren<AnimatedComponent>();
        }

        private void Start()
        {
            m_frameDuration = 1.0f / m_fps;
        }

        private void Update()
        {
            m_timer += Time.deltaTime;

            var updateFrame = false;
            while (m_timer >= m_frameDuration)
            {
                m_timer -= m_frameDuration;
                updateFrame = true;
            }
            if(!updateFrame) { return; }

            UpdateFrame();
        }

        private void UpdateFrame()
        {
            m_currentFrame++;
            foreach (var component in m_components)
            {
                component.UpdateFrame(m_currentFrame);
            }
        }
    }
}