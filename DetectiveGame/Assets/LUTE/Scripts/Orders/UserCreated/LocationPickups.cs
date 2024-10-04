using MoreMountains.Feedbacks;
using MoreMountains.InventoryEngine;
using UnityEngine;

[OrderInfo("Items",
              "LocationPickups",
              "Creates a pickup item on the map and will either show this as a card with button or auto pickup")]
[AddComponentMenu("")]
public class LocationPickups : Order
{
    [Tooltip("The feedback to play when the item gets picked up")]
    [SerializeField] protected MMFeedbacks pickupFeedback;
    [Tooltip("Whether or not to show the quantity prompt")]
    [SerializeField] protected bool showPrompt = true;
    [Tooltip("Whether or not to show the item pickup card (if false the item wil be picked up automatically)")]
    [SerializeField] protected bool showPickupCard = true;
    [Tooltip("Where this item will be placed on the map")]
    [SerializeField] protected LocationVariable itemLocation;
    [Tooltip("The item that will actually be picked up")]
    [SerializeField] protected InventoryItem item;
    [Tooltip("How many of the item will be picked up")]
    [SerializeField] protected int itemsQuantitiy;
    public override void OnEnter()
    {
        LocationItemPickup.CreateItem(null, item, itemsQuantitiy, pickupFeedback, showPrompt, showPickupCard, itemLocation, ParentNode);
        Continue();
    }

    public override string GetSummary()
    {
        string cardText = showPickupCard ? "display a pickup card" : "pickup " + itemsQuantitiy + " automatically";
        return "Create a " + item.ItemName + " pickup that will " + cardText;
    }

}