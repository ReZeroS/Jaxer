using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    health,
    critiChance,
    critPower,
    armor,
    evasion,
    magicRes,
    fireDamage,
    lightningDamage
}


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
        stats.IncreaseStatBy(buffAmount, buffDuration, StatToModify());
    }

    private Stat StatToModify()
    {
        switch (buffType)
        {
            case StatType.strength:
                return stats.strength;
            case StatType.agility:
                return stats.agility;
            case StatType.intelligence:
                return stats.intelligence;
            case StatType.vitality:
                return stats.vitality;
            case StatType.damage:
                return stats.damage;
            case StatType.health:
                return stats.maxHealth;
            case StatType.critiChance:
                return stats.critChance;
            case StatType.critPower:
                return stats.critPower;
            case StatType.armor:
                return stats.armor;
            case StatType.evasion:
                return stats.evasion;
            case StatType.magicRes:
                return stats.magicResistance;
            case StatType.fireDamage:
                return stats.fireDamage;
            case StatType.lightningDamage:
                return stats.lightingDamage;
            default:
                return null;
        }
    }
}