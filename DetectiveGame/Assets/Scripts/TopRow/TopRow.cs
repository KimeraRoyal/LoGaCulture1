using System;
using MoreMountains.InventoryEngine;
using UnityEngine;

namespace KR.TopRow
{
    public class TopRow : MonoBehaviour
    {
        private Profile m_profile;
        private InventoryInputManager m_inventory;
        private XRHelper m_xrHelper;
        private SaveMenu m_saveMenu;

        private Openable m_topRowOpenable;
        private Openable m_backOpenable;

        private int m_showTrackerCount;

        private void Awake()
        {
            m_profile = FindAnyObjectByType<Profile>();
            m_inventory = FindAnyObjectByType<InventoryInputManager>();
            m_xrHelper = FindAnyObjectByType<XRHelper>();
            m_saveMenu = FindAnyObjectByType<SaveMenu>(FindObjectsInactive.Include);

            var openables = GetComponents<Openable>();
            m_topRowOpenable = openables[0];
            m_backOpenable = openables[1];
        
            m_profile.OnOpened += OnEnterSubmenu;
            m_profile.OnClosed += OnExitSubmenu;
        
            m_inventory.OnOpened += OnEnterSubmenu;
            m_inventory.OnClosed += OnExitSubmenu;
        
            m_xrHelper.OnEnabled += OnEnterSubmenu;
            m_xrHelper.OnDisabled += OnExitSubmenu;

            m_topRowOpenable.OnOpened += () => { m_saveMenu.gameObject.SetActive(true); };
            m_topRowOpenable.OnClosed += () => { m_saveMenu.gameObject.SetActive(false); };
        }

        private void Start()
        {
            m_showTrackerCount = m_topRowOpenable.Opened ? 1 : 0;
        }

        public void Close()
        {
            m_profile.Open = false;
            m_inventory.CloseInventory();
            m_xrHelper.SetXREnabled(false);
        }

        public void ShowTopRow(bool _show)
        {
            var previousTrackerCount = m_showTrackerCount;
            m_showTrackerCount += _show ? 1 : -1;
            
            // Were no trackers before, are now. Open up.
            if (previousTrackerCount < 1 && m_showTrackerCount > 0)
            {
                m_topRowOpenable.Open();
            }
            
            // Were trackers before, aren't anymore. Close.
            if (previousTrackerCount > 0 && m_showTrackerCount < 1)
            {
                m_topRowOpenable.Close();
            }
        }

        private void OnEnterSubmenu()
        {
            ShowTopRow(false);
            m_backOpenable.Open();
        }

        private void OnExitSubmenu()
        {
            ShowTopRow(true);
            m_backOpenable.Close();
        }
    }
}
