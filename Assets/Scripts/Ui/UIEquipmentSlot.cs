using UnityEngine.EventSystems;

public class UIEquipmentSlot : UIItemSlot
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name = "Equipment slot + " + slotType;
    }


    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
        {
            return;
        }
        Inventory.instance.UnEquipmentItem(item.data as ItemDataEquipment);
        Inventory.instance.AddItem(item.data as ItemDataEquipment);
        CleanUpSlot();
    }
}
