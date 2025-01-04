using TMPro;
using UnityEngine;

public class UIItemtooltip : UITooltip
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemDescription;

    public void ShowTooltip(ItemDataEquipment itemData, Vector2 pointerPosition)
    {
        if (itemData == null)
        {
            return;
        }
        AdjustPosition(pointerPosition);
        itemName.text = itemData.itemName;
        itemType.text = itemData.itemType.ToString();
        itemDescription.text = itemData.GetDescription();
         
        gameObject.SetActive(true);
    }




    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
    
}
