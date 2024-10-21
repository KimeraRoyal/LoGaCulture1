using MoreMountains.InventoryEngine;
using UnityEngine;

namespace KR.TopRow
{
    public class TopRow : MonoBehaviour
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

        public void Close()
        {
            m_profile.Open = false;
            m_inventory.CloseInventory();
        }
    }
}
