using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//todo use gamepad controller 
public class UIItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    public InventoryItem item;

    protected UI ui;

    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    public void UpdateSlot(InventoryItem newItem)
    {
        item = newItem;

        itemImage.color = Color.white;

        if (item.data != null)
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


    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
        {
            return;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }

        if (item.data.itemType == ItemType.Equipment)
        {
            Inventory.instance.EquipItem(item.data);
        }

        ui.itemTooltip.HideTooltip();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
        {
            return;
        }
        ui.itemTooltip.ShowTooltip(item.data as ItemDataEquipment, eventData.position);
      }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.itemTooltip.HideTooltip();
    }
}