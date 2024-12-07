using ReZeros.Jaxer.Manager;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff effect", menuName = "Data/Item effect/Buff effect")]
public class BuffEffects : ItemEffects
{
    private PlayerStat stats;
    [SerializeField] private StatType buffType;
    [SerializeField] private int buffAmount;
    [SerializeField] private int buffDuration;


    public override void ExecuteEffect(Transform respawnPosition)
    {
        stats = PlayerManager.instance.player.GetComponent<PlayerStat>();
        stats.IncreaseStatBy(buffAmount, buffDuration, stats.StatOfType(buffType));
    }

    
}