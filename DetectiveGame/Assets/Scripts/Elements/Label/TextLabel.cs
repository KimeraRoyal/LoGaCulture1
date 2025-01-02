using TMPro;
using UnityEngine;

namespace KR.Elements.Label
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextLabel : MonoBehaviour, ILabelledElement
    {
        private TMP_Text m_text;

        [SerializeField] private string m_textFormat = "{0}";

        private void Awake()
            => m_text = GetComponent<TMP_Text>();

        public string GetLabel()
            => m_text.text;

        public void SetLabel(string _label)
            => m_text.text = string.Format(m_textFormat, _label);

        public float GetFontSize()
            => m_text.fontSize;

        public void SetFontSize(float _fontSize)
            => m_text.fontSize = _fontSize;
    }
}