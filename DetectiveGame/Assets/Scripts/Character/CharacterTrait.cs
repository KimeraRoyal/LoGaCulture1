using System;
using KW.Flags;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class CharacterTrait
{
    [SerializeField] private NamedFlags m_namedFlags;
    
    [SerializeField] private string m_text = "Something describing this character.";
    
    [SerializeField] private string m_requiredFlag;
    [SerializeField] private int m_requiredFlagNameIndex = -1;
    
    [SerializeField] private int m_lastNamedFlagCount = -1;

    public string Text => m_text;

    public bool CheckValidity(NamedFlags _namedFlags)
        => string.IsNullOrEmpty(m_requiredFlag) || _namedFlags.IsFlagSet(m_requiredFlag);

    public void OnValidate(NamedFlags _namedFlags)
    {
        if(!m_namedFlags) { m_namedFlags = _namedFlags; }
        
        m_requiredFlag = m_requiredFlag?.ToLower();
        ValidateFlagName(_namedFlags);
    }

    private void ValidateFlagName(NamedFlags _namedFlags)
    {
        var flagNames = _namedFlags.FlagNames;
        
        if (string.IsNullOrEmpty(m_requiredFlag))
        {
            m_requiredFlagNameIndex = -1;
            m_lastNamedFlagCount = flagNames.Count;
            
            return;
        }

        for (var i = 0; i < flagNames.Count; i++)
        {
            if(flagNames[i] != m_requiredFlag) { continue; }
            
            m_requiredFlagNameIndex = i;
            m_lastNamedFlagCount = flagNames.Count;
            
            return;
        }

        var validIndex = m_requiredFlagNameIndex >= 0 && m_lastNamedFlagCount == flagNames.Count;
        m_requiredFlag = validIndex ? flagNames[m_requiredFlagNameIndex] : "";
    }
}