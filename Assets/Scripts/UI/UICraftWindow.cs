using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICraftWindow : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI itemName;
   [SerializeField] private TextMeshProUGUI itemDescription;
   [SerializeField] private Image itemIcon;
   [SerializeField] private Button craftButton;

   [SerializeField] private Image[] materialImageList;

   public void SetupCraftWindow(ItemDataEquipment itemEquipment)
   {
      Debug.Log("SetupCraftWindow up craft window for " + itemEquipment.name);
      craftButton.onClick.RemoveAllListeners();

      for (int i = 0; i < materialImageList.Length; i++)
      {
         materialImageList[i].color = Color.clear;
         materialImageList[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
      }

      for (int i = 0; i < itemEquipment.craftingMaterials.Count; i++)
      {
         if (itemEquipment.craftingMaterials.Count > materialImageList.Length)
         {
            Debug.LogWarning("you have more crafting materials than material slots in craft window");
         }
         
         
         materialImageList[i].color = Color.white;
         materialImageList[i].sprite = itemEquipment.craftingMaterials[i].data.icon;


         TextMeshProUGUI materialSlotText = materialImageList[i].GetComponentInChildren<TextMeshProUGUI>();
         materialSlotText.text = itemEquipment.craftingMaterials[i].stackSize.ToString();
         materialSlotText.color = Color.white;
      }
      
      itemIcon.sprite = itemEquipment.icon;
      itemName.text = itemEquipment.name;
      itemDescription.text = itemEquipment.GetDescription();
      craftButton.onClick.AddListener(() => Inventory.instance.CanCraft(itemEquipment, itemEquipment.craftingMaterials));
   }

}
