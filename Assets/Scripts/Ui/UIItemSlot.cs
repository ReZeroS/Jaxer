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

        Vector2 pointerPosition = eventData.position;

        var halfScreenWidth = Screen.width / 2;
        float xOffset, yOffset;
        if (pointerPosition.x > halfScreenWidth)
        {
            xOffset = -150;
        }
        else
        {
            xOffset = 150;
        }

        var halfScreenHeight = Screen.height / 2;
        if (pointerPosition.y > halfScreenHeight)
        {
            yOffset = -150;
        }
        else
        {
            yOffset = 150;
        }

        ui.itemTooltip.ShowTooltip(item.data as ItemDataEquipment);
        ui.itemTooltip.transform.position = new Vector2(pointerPosition.x + xOffset, pointerPosition.y + yOffset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.itemTooltip.HideTooltip();
    }
}