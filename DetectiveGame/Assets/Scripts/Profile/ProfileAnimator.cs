using KR;
using UnityEngine;

[RequireComponent(typeof(Openable))]
public class ProfileAnimator : MonoBehaviour
{
    private Profile m_profile;

    private Openable m_openable;

    private void Awake()
    {
        m_profile = GetComponentInParent<Profile>();
        
        m_openable = GetComponent<Openable>();
        
        m_profile.OnOpened += m_openable.Open;
        m_profile.OnClosed += m_openable.Close;
    }
}
