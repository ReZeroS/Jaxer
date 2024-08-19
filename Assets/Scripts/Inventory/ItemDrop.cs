using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemDrop : MonoBehaviour
{

    [SerializeField] private int possibleItemDrop;
    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> dropList = new();
    [SerializeField] private GameObject dropPrefab;


    public void GenerateDrop()
    {
        foreach (var item in possibleDrop)
        {
            if (Random.Range(0, 100) < item.dropChance)
            {
                dropList.Add(item);
            }
        }

        
        for (int i = 0; i < possibleItemDrop; i++)
        {
            if (dropList.Count <= 0)
            {
                return;
            }
            ItemData randomItem = dropList[Random.Range(0, dropList.Count - 1)];
            dropList.Remove(randomItem);
            DropItem(randomItem);
        }
    }
    
    
    public void DropItem(ItemData itemData)
    {
        GameObject drop = Instantiate(dropPrefab, transform.position, Quaternion.identity);
        Vector2 randomVelocity = new Vector2(Random.Range(-5f, 5f), Random.Range(12f, 15f));
        drop.GetComponent<ItemObject>().SetupItem(itemData, randomVelocity);
    }
    
    
}
