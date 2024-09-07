using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICraftList : MonoBehaviour, IPointerDownHandler
{
    
    [SerializeField] private Transform craftSlotParent;
    [SerializeField] private GameObject craftSlotPrefab;

    [SerializeField] private List<ItemDataEquipment> craftEquipmentList;



    private void Start()
    {
        transform.parent.GetChild(0).GetComponent<UICraftList>().SetupCraftList();
        SetupDefaultCraftWindow();
    }
    

    private void SetupCraftList()
    {
        for (int i = 0; i < craftSlotParent.childCount; i++)
        {
            Destroy(craftSlotParent.GetChild(i).gameObject);
        }

        foreach (var t in craftEquipmentList)
        {
            Debug.Log(t.name);
            GameObject newCraftSlot = Instantiate(craftSlotPrefab, craftSlotParent);
            UICraftSlot craftSlot = newCraftSlot.GetComponent<UICraftSlot>();
            craftSlot.SetupCraftSlot(t);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCraftList();
    }


    private void SetupDefaultCraftWindow()
    {
        if (craftEquipmentList[0])
        {
            GetComponentInParent<UI>().craftWindow.SetupCraftWindow(craftEquipmentList[0]);
        }
    }
    
    
}
