using UnityEngine;

public class Suitcase : MonoBehaviour
{
    private Animator m_animator;

    [SerializeField] private string m_openParameterName = "Open";
    private int m_openHash;
    
    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        
        m_openHash = Animator.StringToHash(m_openParameterName);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            m_animator.SetBool(m_openHash, !m_animator.GetBool(m_openHash));
        }
    }
}
