using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//todo use gamepad controller 
public class UIItemSlot : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    public InventoryItem item;



    public void UpdateSlot(InventoryItem newItem)
    {
        
        item = newItem;

        itemImage.color = Color.white;
        
        if (item != null)
        {
            itemImage.sprite = item.data.icon;
            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }


    public void CleanUpSlot()
    {
        item = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }
    
    

    public void OnPointerDown(PointerEventData eventData)
    {
        if (item.data.itemType == ItemType.Equipment)
        {
            Inventory.instance.EquipItem(item.data);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
