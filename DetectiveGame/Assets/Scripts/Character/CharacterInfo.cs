using System.Collections.Generic;
using System.Linq;
using KW.Flags;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    private NamedFlags m_namedFlags;
    
    [SerializeField] private string m_job = "???";

    [SerializeField] private List<CharacterTrait> m_traits;

    private NamedFlags NamedFlags
    {
        get
        {
            if(!m_namedFlags) { m_namedFlags = FindAnyObjectByType<NamedFlags>(); }
            return m_namedFlags;
        }
    }

    public string Job => m_job;

    public IReadOnlyList<CharacterTrait> Traits => m_traits;

    public List<string> GetValidTraits()
    {
        var validTraits = (from trait in m_traits where trait.CheckValidity(NamedFlags) select trait.Text).ToList();
        if(validTraits.Count < 1) { validTraits.Add("???"); }
        return validTraits;
    }

    private void OnValidate()
    {
        m_traits ??= new List<CharacterTrait>();
        foreach (var trait in m_traits)
        {
            trait.OnValidate(NamedFlags);
        }
    }
}
