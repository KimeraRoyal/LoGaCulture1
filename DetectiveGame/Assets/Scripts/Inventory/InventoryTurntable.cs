using MoreMountains.InventoryEngine;
using UnityEngine;

public class InventoryTurntable : MonoBehaviour
{
    private InventoryDetails m_inventory;

    private SpriteRenderer m_renderer;

    private void Awake()
    {
        m_inventory = FindAnyObjectByType<InventoryDetails>();

        m_renderer = GetComponent<SpriteRenderer>();
        
        m_inventory.OnDetailsFilled += OnDetailsFilled;
    }

    private void OnDetailsFilled(InventoryItem _item)
    {
        m_renderer.sprite = _item.Icon;
    }
}
