using UnityEngine;

public class EnemyStat : CharacterStat
{
    private Enemy enemy;
    private ItemDrop myDropSystem;

    [Header("Level details")]
    [SerializeField] private int level = 1;

    [Range(0, 1f)] [SerializeField] private float percentageModifier = .4f;


    protected override void Start()
    {
        ApplyModifiers();

        base.Start();
        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();
    }

    private void ApplyModifiers()
    {
        Modifer(strength);
        Modifer(agility);
        Modifer(intelligence);
        Modifer(vitality);
        
        
        Modifer(damage);
        Modifer(critChance);
        Modifer(critPower);
        
        
        Modifer(maxHealth);
        Modifer(armor);
        Modifer(evasion);
        Modifer(magicResistance);
        
        
        Modifer(fireDamage);
        Modifer(iceDamage);
        Modifer(lightingDamage);
    }


    private void Modifer(Stat stat)
    {
        for (int i = 0; i < level; i++)
        {
            float modifier = stat.GetValue() * percentageModifier;
            stat.AddModifer((int)modifier);
        }
    }


    public override void TakeDamage(int dam)
    {
        base.TakeDamage(dam);
    }

    public override void Die()
    {
        base.Die();
        enemy.Die();
        myDropSystem.GenerateDrop();
    }
}