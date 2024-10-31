using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LUTE.Scripts.Orders.Dialogue
{
    public class Characters : MonoBehaviour
    {
        private List<Character> m_characters;
        private List<Character> m_unlockedCharacters;

        public IReadOnlyList<Character> AllCharacters => m_characters;
        public IReadOnlyList<Character> UnlockedCharacters => m_unlockedCharacters;
        
        public Action<Character, bool> OnCharacterUnlocked;
        public Action OnUnlockedCharactersChanged;

        private bool m_dirty;
        
        private void Awake()
        {
            m_characters = GetComponentsInChildren<Character>().ToList();
            foreach (var character in m_characters)
            {
                character.OnUnlockedChanged += unlocked => CharacterUnlocked(character, unlocked);
            }
            UpdateUnlockedCharacters();
        }

        private void Update()
        {
            if(!m_dirty) { return; }
            UpdateUnlockedCharacters();
        }

        private void UpdateUnlockedCharacters()
        {
            m_unlockedCharacters = m_characters.Where(character => character.Unlocked).ToList();
            m_dirty = false;
        }

        private void CharacterUnlocked(Character _character, bool _unlocked)
        {
            OnCharacterUnlocked?.Invoke(_character, _unlocked);
            m_dirty = true;
        }
    }
}