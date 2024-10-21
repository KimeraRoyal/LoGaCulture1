using KR;
using MoreMountains.InventoryEngine;
using UnityEngine;

[RequireComponent(typeof(Openable))]
public class Suitcase : MonoBehaviour
{
    private InventoryInputManager m_inventoryInputManager;

    private Openable m_openable;
    
    private void Awake()
    {
        m_inventoryInputManager = FindAnyObjectByType<InventoryInputManager>();
        
        m_openable = GetComponent<Openable>();
        
        m_inventoryInputManager.OnOpened += m_openable.Open;
        m_inventoryInputManager.OnClosed += m_openable.Close;
    }
}
