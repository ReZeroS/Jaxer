using ReZeros.Jaxer.Manager;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item effect/Heal effect")]
public class HealEffects : ItemEffects
{

    [SerializeField] private float healPercent;
    
    public override void ExecuteEffect(Transform respawnPosition)
    {
        PlayerStat playerStat = PlayerManager.instance.player.GetComponent<PlayerStat>();

        int healAmount = (int)(playerStat.GetMaxHealthVal() * healPercent);
        
        playerStat.IncreaseHealthBy(healAmount);
    }
}
