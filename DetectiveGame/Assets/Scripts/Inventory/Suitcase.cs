using MoreMountains.InventoryEngine;
using UnityEngine;

public class Suitcase : MonoBehaviour
{
    private InventoryInputManager m_inventoryInputManager;
    
    private Animator m_animator;

    [SerializeField] private string m_openParameterName = "Open";
    private int m_openHash;

    private bool m_open;
    
    private void Awake()
    {
        m_inventoryInputManager = FindAnyObjectByType<InventoryInputManager>();
        m_inventoryInputManager.OnOpen += OnOpen;
        m_inventoryInputManager.OnClose += OnClose;
        
        m_animator = GetComponent<Animator>();
        
        m_openHash = Animator.StringToHash(m_openParameterName);
    }

    private void OnOpen()
        => SetOpen(true);

    private void OnClose()
        => SetOpen(false);

    private void SetOpen(bool _open)
    {
        if(m_open == _open) { return; }
        m_animator.SetBool(m_openHash, !m_animator.GetBool(m_openHash));
        m_open = _open;
    }
}
