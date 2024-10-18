using System;
using TMPro;
using UnityEngine;

public class ProfileText : MonoBehaviour
{
    private enum Category
    {
        Name,
        Job,
        Trait
    }
    
    private Profile m_profile;

    private TMP_Text m_text;

    [SerializeField] private string m_format = "{0}";
    [SerializeField] private Category m_category;

    private void Awake()
    {
        m_profile = GetComponentInParent<Profile>();
        m_profile.OnCharacterSelected += OnCharacterSelected;
        m_profile.OnOpened += OnOpened;

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
                Category.Job => _character.Info.Job,
                Category.Trait => _character.Info.GetValidTraits()[0],
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        SetText(value);
    }

    private void OnOpened()
    {
        if (m_category == Category.Trait)
        {
            SetText(m_profile.SelectedCharacter.Info.GetValidTraits()[0]);
        }
    }
    
    private void SetText(string _text)
        => m_text.text = string.Format(m_format, _text);
}
