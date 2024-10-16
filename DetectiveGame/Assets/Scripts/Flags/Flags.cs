using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flags : MonoBehaviour
{
    private const int c_sizeInBytes = sizeof(uint);
    private const int c_sizeInBits = c_sizeInBytes * 8;
    
    private List<uint> m_flagBits;

    private void Awake()
    {
        m_flagBits = new List<uint>();
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

    public bool IsFlagSet(int _index)
    {
        var indexInArray = _index / c_sizeInBits;
        if (m_flagBits.Count < indexInArray + 1) { return false; }
        return (m_flagBits[indexInArray] & (uint) 1 << (_index % c_sizeInBits)) > 0;
    }

    public void ClearFlags() 
        => m_flagBits.Clear();
}
