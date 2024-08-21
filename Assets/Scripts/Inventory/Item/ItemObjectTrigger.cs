using UnityEngine;

public class ItemObjectTrigger : MonoBehaviour
{

    private ItemObject getItemObject => GetComponentInParent<ItemObject>();
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<CharacterStat>().isDead)
        {
            return;
        }
        
        
        if (other.GetComponent<Player>() != null)
        {
            getItemObject.PickUpItem();
        }
    }
}
