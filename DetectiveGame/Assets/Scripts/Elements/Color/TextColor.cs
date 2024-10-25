using TMPro;
using UnityEngine;

namespace KR.Elements.Color
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextColor : MonoBehaviour, IColoredElement
    {
        private TMP_Text m_text;

        private void Awake()
            => m_text = GetComponent<TMP_Text>();

        public UnityEngine.Color GetColor()
            => m_text.color;

        public void SetColor(UnityEngine.Color _color)
            => m_text.color = _color;
    }
}