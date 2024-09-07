using TMPro;
using UnityEngine;

public class UIItemtooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemDescription;

    public void ShowTooltip(ItemDataEquipment itemData)
    {
        if (itemData == null)
        {
            return;
        }
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
