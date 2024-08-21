using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{

    [Header("Player drop")]
    [SerializeField] private float changeToLooseItems;


    public override void GenerateDrop()
    {
        // list of equipment 
        Inventory inventory = Inventory.instance;
        List<InventoryItem> inventoryItems = inventory.GetEquipmentList();
        List<InventoryItem> toUnEquipmentList = inventory.GetEquipmentList();
        // foreach item we goona check if should loose item
        foreach (InventoryItem inventoryItem in inventoryItems)
        {
            if (Random.Range(0, 100) < changeToLooseItems)
            {
                DropItem(inventoryItem.data);
                toUnEquipmentList.Add(inventoryItem);
            }
        }

        for (var index = 0; index < toUnEquipmentList.Count; index++)
        {
            var inventoryItem = toUnEquipmentList[index];
            inventory.UnEquipmentItem(inventoryItem.data as ItemDataEquipment);
        }
    }


}
