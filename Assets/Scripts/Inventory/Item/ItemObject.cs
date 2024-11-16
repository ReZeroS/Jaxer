using Sound.SoundManager;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private Rigidbody2D rb;

    private void SetupVisuals()
    {
        if (itemData == null)
        {
            return;
        }
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object-" + itemData.itemName;
    }
    
    
    public void SetupItem(ItemData item, Vector2 vel)
    {
        itemData = item;
        rb.linearVelocity = vel;
        SetupVisuals();
    }


    public void PickUpItem()
    {
        if (!Inventory.instance.CanAddItem() && itemData.itemType == ItemType.Equipment)
        {
            rb.linearVelocity = new Vector2(0, 7);
            return;
        }
        
        SoundManager.PlaySound3d(SoundType.PICKUP);
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
