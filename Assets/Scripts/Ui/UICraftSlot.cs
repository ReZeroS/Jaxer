using UnityEngine.EventSystems;

public class UICraftSlot : UIItemSlot
{
    private void OnEnable()
    {
        UpdateSlot(item);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemDataEquipment itemToCraft = item.data as ItemDataEquipment;
        if (itemToCraft == null)
        {
            return;
        }
        Inventory.instance.CanCraft(itemToCraft, itemToCraft.craftingMaterials);
    }
}