using System;
using UnityEngine;

public class ProfileTrait : MonoBehaviour
{
    private Profile m_profile;

    private CanvasGroup m_canvasGroup;

    [SerializeField] private int m_traitIndex = -1;

    public int TraitIndex
    {
        get => m_traitIndex;
        set => m_traitIndex = value;
    }

    public Action<string> OnTraitChanged;

    private void Awake()
    {
        m_profile = GetComponentInParent<Profile>();
        m_profile.OnCharacterSelected += OnCharacterSelected;
        m_profile.OnOpened += OnOpened;

        m_canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnCharacterSelected(Character _character)
        => UpdateTrait(_character.Info);
    
    private void OnOpened()
        => UpdateTrait(m_profile.SelectedCharacter.Info);

    private void UpdateTrait(CharacterInfo _info)
    {
        if(m_traitIndex < 0) { return; }

        var traits = _info.GetValidTraits();
        var inBounds = traits.Count > m_traitIndex;
        
        string trait = null;
        if (inBounds) { trait = traits[m_traitIndex]; }
        OnTraitChanged?.Invoke(trait);
        
        m_canvasGroup.alpha = inBounds ? 1.0f : 0.0f;
    }
}
