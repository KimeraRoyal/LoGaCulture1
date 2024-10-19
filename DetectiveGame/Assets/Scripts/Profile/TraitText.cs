using TMPro;
using UnityEngine;

public class TraitText : MonoBehaviour
{
    private ProfileTrait m_profileTrait;

    private TMP_Text m_text;

    [SerializeField] private string m_format = "{0}";

    private void Awake()
    {
        m_profileTrait = GetComponentInParent<ProfileTrait>();
        m_profileTrait.OnTraitChanged += SetText;

        m_text = GetComponent<TMP_Text>();
    }
    
    private void SetText(string _text)
        => m_text.text = string.Format(m_format, _text);
}
