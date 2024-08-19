using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}


[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemDataEquipment : ItemData
{
    public EquipmentType equipmentType;

    [Header("Major ints")]
    public int strength;

    public int agility;
    public int intelligence;
    public int vitality;

    [Header("Offensive int")]
    public int damage;

    public int critChance;
    public int critPower;


    [Header("Defensive ints")]
    public int health;

    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("Magic ints")]
    public int fireDamage;

    public int iceDamage;
    public int lightingDamage;


    public void AddModifiers()
    {
        PlayerStat playerStat = PlayerManager.instance.player.GetComponent<PlayerStat>();

        playerStat.strength.AddModifer(strength);
        playerStat.agility.AddModifer(agility);
        playerStat.intelligence.AddModifer(intelligence);
        playerStat.vitality.AddModifer(vitality);

        playerStat.damage.AddModifer(damage);
        playerStat.critChance.AddModifer(critChance);
        playerStat.critPower.AddModifer(critPower);

        playerStat.maxHealth.AddModifer(health);
        playerStat.armor.AddModifer(armor);
        playerStat.evasion.AddModifer(evasion);
        playerStat.magicResistance.AddModifer(magicResistance);
        
        playerStat.fireDamage.AddModifer(fireDamage);
        playerStat.iceDamage.AddModifer(iceDamage);
        playerStat.lightingDamage.AddModifer(lightingDamage);
        
    }


    public void RemoveModifiers()
    {
        PlayerStat playerStat = PlayerManager.instance.player.GetComponent<PlayerStat>();

        playerStat.strength.RemoveModifer(strength);
        playerStat.agility.RemoveModifer(agility);
        playerStat.intelligence.RemoveModifer(intelligence);
        playerStat.vitality.RemoveModifer(vitality);
        playerStat.damage.RemoveModifer(damage);
        playerStat.critChance.RemoveModifer(critChance);
        playerStat.critPower.RemoveModifer(critPower);
        playerStat.maxHealth.RemoveModifer(health);
        playerStat.armor.RemoveModifer(armor);
        playerStat.evasion.RemoveModifer(evasion);
        playerStat.magicResistance.RemoveModifer(magicResistance);
        playerStat.fireDamage.RemoveModifer(fireDamage);
        playerStat.iceDamage.RemoveModifer(iceDamage);
        playerStat.lightingDamage.RemoveModifer(lightingDamage);
    }
    
    
}