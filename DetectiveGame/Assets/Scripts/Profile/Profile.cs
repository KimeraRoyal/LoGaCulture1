using System;
using UnityEngine;

public class Profile : MonoBehaviour
{
    private Character[] m_characters;

    private int m_characterIndex = -1;

    public Character SelectedCharacter
    {
        get
        {
            if (m_characterIndex < 0 || m_characterIndex >= m_characters.Length) { return null; }
            return m_characters[m_characterIndex];
        }
        set => Select(value);
    }
    
    public Action<Character> OnCharacterSelected;
    
    private void Awake()
    {
        m_characters = FindObjectsByType<Character>(FindObjectsSortMode.None);
    }

    private void Start()
    {
        Select(0);
    }

    public void Select(Character _character)
    {
        var index = -1;
        for (var i = 0; i < m_characters.Length; i++)
        {
            if (m_characters[i] != _character) { continue; }
            
            index = i;
            break;
        }
        Select(index);
    }

    public void Select(int _index)
    {
        if (_index < 0 || _index >= m_characters.Length) { return; }

        m_characterIndex = _index;
        OnCharacterSelected?.Invoke(SelectedCharacter);
    }
}
