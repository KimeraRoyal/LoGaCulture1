using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ProfileAnimator : MonoBehaviour
{
    private Profile m_profile;

    private Animator m_animator;

    [SerializeField] private string m_openParameterName = "Open";
    private int m_openHash;

    private void Awake()
    {
        m_profile = GetComponentInParent<Profile>();
        
        m_profile.OnOpened += OnOpened;
        m_profile.OnClosed += OnClosed;

        m_animator = GetComponent<Animator>();
        m_openHash = Animator.StringToHash(m_openParameterName);
    }

    private void OnOpened()
        => OnOpenChanged(true);

    private void OnClosed()
        => OnOpenChanged(false);

    private void OnOpenChanged(bool _open)
        => m_animator.SetBool(m_openHash, _open);
}
