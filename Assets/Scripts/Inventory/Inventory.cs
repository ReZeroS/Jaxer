using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Inventory : MonoBehaviour, ISaveManager
{
    public static Inventory instance;


    public List<InventoryItem> equipmentItems;
    public Dictionary<ItemDataEquipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventoryItems = new();
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;


    public List<InventoryItem> stashItems;
    public Dictionary<ItemData, InventoryItem> stashDictionary;


    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;

    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private UIItemSlot[] inventoryItemSlots;
    private UIItemSlot[] stashItemSlots;
    private UIEquipmentSlot[] equipmentItemSlots;
    private UIStatSlot[] statSlots;

    [Header("Database")]
    public List<ItemData> itemDatabase;

    public List<InventoryItem> loadedItems;

    public List<ItemDataEquipment> loadedEquipments;


    [Header("Start Equipment")]
    public List<ItemData> startingEquipment;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        inventoryItems = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
        stashItems = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();
        equipmentItems = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemDataEquipment, InventoryItem>();

        inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<UIItemSlot>();
        stashItemSlots = stashSlotParent.GetComponentsInChildren<UIItemSlot>();
        equipmentItemSlots = equipmentSlotParent.GetComponentsInChildren<UIEquipmentSlot>();
        statSlots = statSlotParent.GetComponentsInChildren<UIStatSlot>();

        AddStartingItems();
    }

    private void AddStartingItems()
    {
        foreach (var loadedEquipment in loadedEquipments)
        {
            EquipItem(loadedEquipment);
        }


        if (loadedItems.Count > 0)
        {
            foreach (var inventoryItem in loadedItems)
            {
                for (int i = 0; i < inventoryItem.stackSize; i++)
                {
                    AddItem(inventoryItem.data);
                }
            }

            return;
        }


        for (var i = 0; i < startingEquipment.Count; i++)
        {
            if (startingEquipment[i])
            {
                AddItem(startingEquipment[i]);
            }
        }
    }

    public void EquipItem(ItemData itemData)
    {
        ItemDataEquipment newEquipment = itemData as ItemDataEquipment;
        InventoryItem newItem = new InventoryItem(itemData);

        ItemDataEquipment oldEquipment = null;
        foreach (var (key, _) in equipmentDictionary)
        {
            if (key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = key;
            }
        }

        if (oldEquipment != null)
        {
            UnEquipmentItem(oldEquipment);
            AddItem(oldEquipment);
        }

        equipmentItems.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();
        RemoveItem(itemData);
        UpdateSlotUI();
    }

    public void UnEquipmentItem(ItemDataEquipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipmentItems.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();
        }
    }


    private void UpdateSlotUI()
    {
        for (var i = 0; i < equipmentItemSlots.Length; i++)
        {
            foreach (var (key, val) in equipmentDictionary)
            {
                if (key.equipmentType == equipmentItemSlots[i].slotType)
                {
                    equipmentItemSlots[i].UpdateSlot(val);
                }
            }
        }


        for (var i = 0; i < inventoryItemSlots.Length; i++)
        {
            inventoryItemSlots[i].CleanUpSlot();
        }

        for (var i = 0; i < stashItemSlots.Length; i++)
        {
            stashItemSlots[i].CleanUpSlot();
        }


        for (int i = 0; i < inventoryItems.Count; i++)
        {
            inventoryItemSlots[i].UpdateSlot(inventoryItems[i]);
        }

        for (int i = 0; i < stashItems.Count; i++)
        {
            stashItemSlots[i].UpdateSlot(stashItems[i]);
        }


        UpdateStatSlotsUI();
    }

    public void UpdateStatSlotsUI()
    {
        for (int i = 0; i < statSlots.Length; i++)
        {
            statSlots[i].UpdateStatValueUI();
        }
    }


    public void AddItem(ItemData item)
    {
        if (item.itemType == ItemType.Equipment && CanAddItem())
        {
            AddToInventory(item);
        }
        else if (item.itemType == ItemType.Material)
        {
            AddToStash(item);
        }

        UpdateSlotUI();
    }

    private void AddToStash(ItemData item)
    {
        if (stashDictionary.TryGetValue(item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            stashItems.Add(newItem);
            stashDictionary.Add(item, newItem);
        }
    }

    private void AddToInventory(ItemData item)
    {
        if (inventoryDictionary.TryGetValue(item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            inventoryItems.Add(newItem);
            inventoryDictionary.Add(item, newItem);
        }
    }

    public void RemoveItem(ItemData item)
    {
        if (inventoryDictionary.TryGetValue(item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventoryItems.Remove(value);
                inventoryDictionary.Remove(item);
            }
            else
            {
                value.RemoveStack();
            }
        }

        if (stashDictionary.TryGetValue(item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stashItems.Remove(stashValue);
                stashDictionary.Remove(item);
            }
            else
            {
                stashValue.RemoveStack();
            }
        }


        UpdateSlotUI();
    }

    public bool CanAddItem()
    {
        if (inventoryItems.Count >= inventoryItemSlots.Length)
        {
            return false;
        }

        return true;
    }


    public bool CanCraft(ItemDataEquipment itemToCraft, List<InventoryItem> requireMaterials)
    {
        Debug.Log("CanCraft Start");
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();
        for (var i = 0; i < requireMaterials.Count; i++)
        {
            var itemData = requireMaterials[i].data;
            if (stashDictionary.TryGetValue(itemData, out InventoryItem stashValue))
            {
                if (stashValue.stackSize < requireMaterials[i].stackSize)
                {
                    return false;
                }

                materialsToRemove.Add(stashValue);
            }
            else
            {
                return false;
            }
        }

        for (var i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].data);
        }

        AddItem(itemToCraft);
        Debug.Log("CanCraft End finished");

        return true;
    }


    public List<InventoryItem> GetEquipmentList()
    {
        return equipmentItems;
    }

    public ItemDataEquipment GetEquipment(EquipmentType equipmentType)
    {
        foreach (var (itemDataEquipment, _) in equipmentDictionary)
        {
            if (itemDataEquipment.equipmentType == equipmentType)
            {
                return itemDataEquipment;
            }
        }

        return null;
    }

    public void UseFlask()
    {
        ItemDataEquipment currentFlask = GetEquipment(EquipmentType.Flask);

        if (!currentFlask)
        {
            return;
        }

        bool canUseFlask = Time.time > currentFlask.lastTimeUsed + currentFlask.itemCooldown;

        if (canUseFlask)
        {
            currentFlask.itemCooldown = currentFlask.itemStartCooldown;
            currentFlask.Effect(null);
            currentFlask.lastTimeUsed = Time.time;
        }
    }


    public bool CanUseArmor()
    {
        ItemDataEquipment currentArmor = GetEquipment(EquipmentType.Armor);
        if (Time.time > currentArmor.lastTimeUsed + currentArmor.itemCooldown)
        {
            currentArmor.itemCooldown = currentArmor.itemStartCooldown;
            currentArmor.lastTimeUsed = Time.time;
            return true;
        }

        return false;
    }

    public void LoadData(GameData gameData)
    {
        List<ItemData> itemDatas = itemDatabase;
        foreach (var (key, value) in gameData.inventory)
        {
            foreach (var itemData in itemDatas)
            {
                if (itemData != null && itemData.itemId == key)
                {
                    InventoryItem newItem = new InventoryItem(itemData);
                    newItem.stackSize = value;
                    loadedItems.Add(newItem);
                }
            }
        }

        foreach (var equipmentId in gameData.equipmentIdList)
        {
            foreach (var itemData in itemDatas)
            {
                if (itemData != null && itemData.itemId == equipmentId)
                {
                    loadedEquipments.Add(itemData as ItemDataEquipment);
                }
            }
        }
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.inventory.Clear();
        gameData.equipmentIdList.Clear();
        foreach (var (key, value) in inventoryDictionary)
        {
            gameData.inventory.Add(key.itemId, value.stackSize);
        }

        foreach (var item in stashDictionary)
        {
            gameData.inventory.Add(item.Key.itemId, item.Value.stackSize);
        }

        foreach (var item in equipmentDictionary)
        {
            gameData.equipmentIdList.Add(item.Key.itemId);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Generate Item Database")]
    public void FillUpItemDatabase() => itemDatabase = new List<ItemData>(GetItemDatabase());

    private List<ItemData> GetItemDatabase()
    {
        List<ItemData> itemDatabase = new List<ItemData>();
        string[] assetsNames = AssetDatabase.FindAssets("", new[] { "Assets/ScriptableObject/Items" });

        foreach (var assetsName in assetsNames)
        {
            var guidToAssetPath = AssetDatabase.GUIDToAssetPath(assetsName);
            var asset = AssetDatabase.LoadAssetAtPath<ItemData>(guidToAssetPath);
            itemDatabase.Add(asset);
        }

        return itemDatabase;
    }
#endif
}