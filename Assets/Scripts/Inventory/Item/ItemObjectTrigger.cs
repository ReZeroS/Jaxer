using ReZeros.Jaxer.PlayerBase;
using UnityEngine;

public class ItemObjectTrigger : MonoBehaviour
{

    private ItemObject getItemObject => GetComponentInParent<ItemObject>();
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        CharacterStat characterStat = other.GetComponent<CharacterStat>();
        if (characterStat == null)
        {
            return;
        }
        if (characterStat.isDead)
        {
            return;
        }
        
        if (other.GetComponent<MainPlayer>() != null)
        {
            getItemObject.PickUpItem();
        }
    }
}
