using UnityEngine;
using UnityEngine.UI;

namespace KR.Elements.Color
{
    [RequireComponent(typeof(Image))]
    public class ImageColor : MonoBehaviour, IColoredElement
    {
        private Image m_image;

        private void Awake()
            => m_image = GetComponent<Image>();

        public UnityEngine.Color GetColor()
            => m_image.color;

        public void SetColor(UnityEngine.Color _color)
            => m_image.color = _color;
    }
}