using UnityEngine;

[CreateAssetMenu(fileName = "Freeze Enemies", menuName = "Data/Item effect/Freeze Enemies")]

public class FreezeEnemyEffects : ItemEffects
{
    [SerializeField] private float freezeDuration;
    
    public override void ExecuteEffect(Transform respawnPosition)
    {
        var playerStat = PlayerManager.instance.player.GetComponent<PlayerStat>();
        // only triggered when player health is less than 10%
        if (playerStat.currentHealth > playerStat.GetMaxHealthVal() * 0.1f)
        {
            return;
        }


        if (!Inventory.instance.CanUseArmor())
        {
            return;
        }
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(respawnPosition.position, 2);
        foreach (Collider2D collider2D in collider2Ds)
        {
            respawnPosition.GetComponent<Enemy>()?.FreezeTimeFor(freezeDuration);
        }
    }
   
    
    
}
