using UnityEngine;
using UnityEngine.UI;

public class ProfilePortrait : MonoBehaviour
{
    private Profile m_profile;

    private Image m_image;

    private void Awake()
    {
        m_profile = GetComponentInParent<Profile>();
        m_profile.OnCharacterSelected += OnCharacterSelected;

        m_image = GetComponent<Image>();
    }

    private void OnCharacterSelected(Character _character)
    {
        m_image.sprite = _character?.GetPortrait(0);
    }
}
