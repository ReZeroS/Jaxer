using UnityEngine;
using UnityEngine.EventSystems;

public class UICraftSlot : UIItemSlot
{
    protected override void Start()
    {
        base.Start();
    }
    
    public void SetupCraftSlot(ItemDataEquipment itemDataEquipment)
    {
        Debug.Log("Setting up craft slot with item: " + itemDataEquipment.name);
        if (!itemDataEquipment)
        {
            return;
        }
        
        item.data = itemDataEquipment;
        itemImage.sprite = itemDataEquipment.icon; 
        itemText.text = itemDataEquipment.name;
    }
    
    
    

    public override void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("craft slot clicked" + item.data.name);
        ui.craftWindow.SetupCraftWindow(item.data as ItemDataEquipment);
    }
}