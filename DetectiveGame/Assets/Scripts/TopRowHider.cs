using KR;
using MoreMountains.InventoryEngine;
using UnityEngine;

public class TopRowHider : MonoBehaviour
{
    private Profile m_profile;
    private InventoryInputManager m_inventory;

    private Openable m_openable;

    private void Awake()
    {
        m_profile = FindAnyObjectByType<Profile>();
        m_inventory = FindAnyObjectByType<InventoryInputManager>();

        m_openable = GetComponent<Openable>();
        
        m_profile.OnOpened += m_openable.Close;
        m_profile.OnClosed += m_openable.Open;
        
        m_inventory.OnOpened += m_openable.Close;
        m_inventory.OnClosed += m_openable.Open;
    }
}
