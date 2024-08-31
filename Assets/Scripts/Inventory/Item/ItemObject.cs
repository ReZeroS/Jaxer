using System;
using System.Collections;
using System.Collections.Generic;
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
        rb.velocity = vel;
        SetupVisuals();
    }


    public void PickUpItem()
    {
        if (!Inventory.instance.CanAddItem() && itemData.itemType == ItemType.Equipment)
        {
            rb.velocity = new Vector2(0, 7);
            return;
        }
        
        
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
