using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace KW.Flags
{
    public class NamedFlags : MonoBehaviour
    {
        private enum InvalidFlagBehaviour
        {
            Disable,
            Warning,
            Enable
        }
    
        private Flags m_flags;

        [SerializeField] protected List<string> m_flagNames;
        private Dictionary<string, int> m_flagNameDictionary;

        [SerializeField] private InvalidFlagBehaviour m_invalidFlagBehaviour;

        public Flags Flags => m_flags;

        public List<string> FlagNames => m_flagNames;
        public IReadOnlyDictionary<string, int> FlagNameDictionary => m_flagNameDictionary;

        private void Awake()
        {
            m_flags = FindAnyObjectByType<Flags>();

            m_flagNameDictionary = new Dictionary<string, int>();
            for (var i = 0; i < m_flagNames.Count; i++)
            {
                m_flagNameDictionary.Add(m_flagNames[i], i);
            }
        }

        public void SetFlag(string _flag)
        {
            var index = GetFlagIndex(_flag);
            if (index < 0) { return; }
            m_flags.SetFlag(index);
        }

        public void ClearFlag(string _flag)
        {
            var index = GetFlagIndex(_flag);
            if (index < 0) { return; }
            m_flags.ClearFlag(index);
        }

        public void ToggleFlag(string _flag)
        {
            var index = GetFlagIndex(_flag);
            if (index < 0) { return; }
            m_flags.ToggleFlag(index);
        }

        public bool IsFlagSet(string _flag)
        {
            var index = GetFlagIndex(_flag);
            return index >= 0 && m_flags.IsFlagSet(index);
        }

        public void ClearFlags()
            => m_flags.ClearFlags();

        public int GetFlagIndex(string _flag)
        {
            _flag = _flag.ToLower();
            
            if (m_flagNameDictionary.TryGetValue(_flag, out var index)) { return index; }
            
            switch (m_invalidFlagBehaviour)
            {
                case InvalidFlagBehaviour.Disable:
                    index = -1;
                    break;
                case InvalidFlagBehaviour.Warning:
                    Debug.LogWarning($"Named flag {_flag} is not found. Adding to dictionary, but be aware this may not be intended.");
                    goto case InvalidFlagBehaviour.Enable;
                case InvalidFlagBehaviour.Enable:
                    index = Add(_flag);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return index;
        }

        private int Add(string _flag)
        {
            var index = m_flagNames.Count;
            m_flagNameDictionary.Add(_flag, index);
            return index;
        }

        private void OnValidate()
        {
            for (var i = 0; i < m_flagNames.Count; i++)
            {
                m_flagNames[i] = m_flagNames[i]?.ToLower();
            }
        }
    }
}
