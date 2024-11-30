using UnityEngine;

namespace KR
{
    public class AnimatedComponent : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer[] m_renderers;
        
        [SerializeField] private Sprite[] m_sprites;
        [SerializeField] private int m_frameOffset;

        public void UpdateFrame(int _index)
        {
            _index += m_frameOffset;
            
            var currentFrame = m_sprites[_index % m_sprites.Length];
            foreach (var spriteRenderer in m_renderers)
            {
                spriteRenderer.sprite = currentFrame;
            }
        }
    }
}
