using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KW.Flags
{
    public class Flags : MonoBehaviour
    {
        public enum Operation
        {
            Set,
            Clear,
            Toggle
        }
        
        private const int c_sizeInBytes = sizeof(uint);
        private const int c_sizeInBits = c_sizeInBytes * 8;
    
        [SerializeField] protected List<uint> m_flagBits;

        public void SetFlag(int _index, bool _flag)
        {
            if(_flag) { SetFlag(_index); }
            else { ClearFlag(_index); }
        }

        public void SetFlag(int _index, Operation _operation)
        {
            switch (_operation)
            {
                case Operation.Set:
                    SetFlag(_index);
                    break;
                case Operation.Clear:
                    ClearFlag(_index);
                    break;
                case Operation.Toggle:
                    ToggleFlag(_index);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_operation), _operation, null);
            }
        }

        public void SetFlag(int _index)
        {
            var indexInArray = _index / c_sizeInBits;
            if (m_flagBits.Count < indexInArray + 1)
            {
                var difference = indexInArray + 1 - m_flagBits.Count;
                m_flagBits.AddRange(Enumerable.Repeat((uint) 0, difference));
            }
            m_flagBits[indexInArray] |= (uint) 1 << (_index % c_sizeInBits);
        }

        public void ClearFlag(int _index)
        {
            var indexInArray = _index / c_sizeInBits;
            if (m_flagBits.Count < indexInArray + 1)
            {
                var difference = indexInArray + 1 - m_flagBits.Count;
                m_flagBits.AddRange(Enumerable.Repeat((uint) 0, difference));
            }
            m_flagBits[indexInArray] &= ~((uint) 1 << (_index % c_sizeInBits));
        }

        public void ToggleFlag(int _index)
        {
            var indexInArray = _index / c_sizeInBits;
            if (m_flagBits.Count < indexInArray + 1)
            {
                var difference = indexInArray + 1 - m_flagBits.Count;
                m_flagBits.AddRange(Enumerable.Repeat((uint) 0, difference));
            }
            m_flagBits[indexInArray] ^= (uint) 1 << (_index % c_sizeInBits);
        }

        public bool IsFlagSet(int _index)
        {
            var indexInArray = _index / c_sizeInBits;
            if (m_flagBits.Count < indexInArray + 1) { return false; }
            return (m_flagBits[indexInArray] & (uint) 1 << (_index % c_sizeInBits)) > 0;
        }

        public void ClearFlags() 
            => m_flagBits.Clear();
    }
}
