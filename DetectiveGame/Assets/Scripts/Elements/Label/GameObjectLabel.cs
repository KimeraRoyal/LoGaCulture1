using UnityEngine;

namespace KR.Elements.Label
{
    public class GameObjectLabel : MonoBehaviour, ILabelledElement
    {
        [SerializeField] private string m_textFormat = "{0}";

        public string GetLabel()
            => gameObject.name;

        public void SetLabel(string _label)
            => gameObject.name = string.Format(m_textFormat, _label);
    }
}