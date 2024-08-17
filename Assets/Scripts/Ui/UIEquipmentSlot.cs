public class UIEquipmentSlot : UIItemSlot
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name = "Equipment slot + " + slotType;
    }
}
