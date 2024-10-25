using UnityEngine;

namespace KR.Elements.Graphic
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteRendererGraphic : MonoBehaviour, IGraphicElement
    {
        private SpriteRenderer m_sprite;

        private void Awake()
            => m_sprite = GetComponent<SpriteRenderer>();

        public UnityEngine.Sprite GetSprite()
            => m_sprite.sprite;

        public void SetSprite(UnityEngine.Sprite _sprite)
            => m_sprite.sprite = _sprite;
    }
}