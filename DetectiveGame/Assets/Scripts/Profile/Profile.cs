using System;
using LUTE.Scripts.Orders.Dialogue;
using UnityEngine;

public class Profile : MonoBehaviour
{
    private Characters m_characters;

    private int m_characterIndex = -1;

    private bool m_open;

    public bool Open
    {
        get => m_open;
        set
        {
            if(m_open == value) { return; }

            m_open = value;
            (m_open ? OnOpened : OnClosed)?.Invoke();
        }
    }

    public Character SelectedCharacter
    {
        get
        {
            if (m_characterIndex < 0 || m_characterIndex >= m_characters.UnlockedCharacters.Count) { return null; }
            return m_characters.UnlockedCharacters[m_characterIndex];
        }
        set => Select(value);
    }
    
    public Action<Character> OnCharacterSelected;

    public Action OnOpened;
    public Action OnClosed;

    public Action OnSelectedPrevious;
    public Action OnSelectedNext;
    
    private void Awake()
    {
        m_characters = FindAnyObjectByType<Characters>();
        m_characters.OnUnlockedCharactersChanged += OnUnlockedCharactersChanged;
    }

    private void OnUnlockedCharactersChanged()
    {
        m_characterIndex = Mathf.Min(m_characterIndex, m_characters.UnlockedCharacters.Count - 1);
        Select(m_characterIndex);
    }

    private void Start()
    {
        Select(0);
    }

    public void Select(Character _character)
    {
        var index = -1;
        for (var i = 0; i < m_characters.UnlockedCharacters.Count; i++)
        {
            if (m_characters.UnlockedCharacters[i] != _character) { continue; }
            
            index = i;
            break;
        }
        Select(index);
    }

    public void Select(int _index)
    {
        if (_index < 0 || _index >= m_characters.UnlockedCharacters.Count) { return; }

        m_characterIndex = _index;
        OnCharacterSelected?.Invoke(SelectedCharacter);
    }

    public void SelectPrevious()
    {
        var index = m_characterIndex - 1;
        if (index < 0) { index += m_characters.UnlockedCharacters.Count; }
        Select(index);
        OnSelectedPrevious?.Invoke();
    }

    public void SelectNext()
    {
        var index = m_characterIndex + 1;
        if (index >= m_characters.UnlockedCharacters.Count) { index -= m_characters.UnlockedCharacters.Count; }
        Select(index);
        OnSelectedNext?.Invoke();
    }

    public void Toggle()
        => Open = !Open;
}
