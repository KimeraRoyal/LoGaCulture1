using UnityEngine;

namespace KR.Elements.Color
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteRendererColor : MonoBehaviour, IColoredElement
    {
        private SpriteRenderer m_sprite;

        private void Awake()
            => m_sprite = GetComponent<SpriteRenderer>();

        public UnityEngine.Color GetColor()
            => m_sprite.color;

        public void SetColor(UnityEngine.Color _color)
            => m_sprite.color = _color;
    }
}