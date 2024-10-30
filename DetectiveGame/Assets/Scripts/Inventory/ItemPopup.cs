using MoreMountains.InventoryEngine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopup : MonoBehaviour
{
    private Inventory m_inventory;

    private Animator m_animator;

    [SerializeField] private TMP_Text m_title;
    [SerializeField] private TMP_Text m_subtitle;

    [SerializeField] private Image m_icon;
    
    [SerializeField] private string m_popupParameterName = "Popup";
    private int m_popupHash;
    
    private void Awake()
    {
        m_inventory = FindAnyObjectByType<Inventory>();
        m_inventory.OnItemAdded += OnItemAdded;

        m_animator = GetComponent<Animator>();
        m_popupHash = Animator.StringToHash(m_popupParameterName);
    }

    private void OnItemAdded(InventoryItem _item)
    {
        m_title.text = _item.ItemName;
        m_subtitle.text = _item.ShortDescription;

        m_icon.sprite = _item.Icon;
        
        m_animator.SetTrigger(m_popupHash);
    }
}
