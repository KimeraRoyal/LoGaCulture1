using System;
using TMPro;
using UnityEngine;

public class ProfileText : MonoBehaviour
{
    private enum Category
    {
        Name,
        Occupation
    }
    
    private Profile m_profile;

    private TMP_Text m_text;

    [SerializeField] private string m_format = "{0}";
    [SerializeField] private Category m_category;

    private void Awake()
    {
        m_profile = GetComponentInParent<Profile>();
        m_profile.OnCharacterSelected += OnCharacterSelected;

        m_text = GetComponent<TMP_Text>();
    }

    private void OnCharacterSelected(Character _character)
    {
        var value = "???";
        if (_character)
        {
            value = m_category switch
            {
                Category.Name => _character.CharacterName,
                Category.Occupation => _character.CharacterOccupation,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        m_text.text = string.Format(m_format, value);
    }
}
