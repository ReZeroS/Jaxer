using System.Collections.Generic;
using ReZeros.Jaxer.Manager;
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

    
    [Header("Unique effects")]
    public List<ItemEffects> itemEffects;
    public float itemCooldown;
    public float lastTimeUsed;
    public float itemStartCooldown;

    [TextArea] public string itemEffectDescription;
    
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

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials;


    private int descriptionLength;
    
    
    public void AddModifiers()
    {
        PlayerStat playerStat = PlayerManager.instance.Player.GetComponent<PlayerStat>();

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
        PlayerStat playerStat = PlayerManager.instance.Player.GetComponent<PlayerStat>();

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

    public void Effect(Transform enemyTransform)
    {
        foreach (ItemEffects itemEffect in itemEffects)
        {
            itemEffect.ExecuteEffect(enemyTransform);
        }
    }


    public override string GetDescription()
    {
        sb.Length = 0;
        

        AddItemDescription(strength, "Strength");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intelligence, "Intelligence");
        AddItemDescription(vitality, "Vitality");
        

        AddItemDescription(damage, "Damage");
        AddItemDescription(critChance, "Crit Chance");
        AddItemDescription(critPower, "Crit Power");

        AddItemDescription(health, "Health");
        AddItemDescription(armor, "Armor");
        AddItemDescription(evasion, "Evasion");
        AddItemDescription(magicResistance, "Magic Resistance");

        AddItemDescription(fireDamage, "Fire Damage");
        AddItemDescription(iceDamage, "Ice Damage");
        AddItemDescription(lightingDamage, "Lighting Damage");
        
        if (descriptionLength < 5)
        {
            for (int i = 0; i < 5 - descriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append(" ");
            }
        }
        
        if (itemEffectDescription.Length > 0)
        {
            sb.AppendLine();
            sb.Append(itemEffectDescription);
        }
        
        return sb.ToString();
    }

    public void AddItemDescription(int val, string name)
    {
        if (val != 0)
        {
            if (sb.Length > 0)
            {
                sb.AppendLine();
            }

            if (val > 0)
            {
                sb.Append(name + ": " + val);
            }

            descriptionLength++;
        }
    }
}