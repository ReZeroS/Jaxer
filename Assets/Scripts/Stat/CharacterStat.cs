using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CharacterStat : MonoBehaviour
{

    [Header("Major stats")]
    public Stat strength;
    public Stat agility;
    public Stat intelligence;
    public Stat vitality;
    
    [Header("Offensive Stat")]
    public Stat damage;
    [FormerlySerializedAs("critChange")] public Stat critChance;
    public Stat critPower;

    
    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;


    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;
    
    
    
    
    


    [SerializeField] private int currentHealth;


    protected virtual void Start()
    {
        critPower.SetDefaultVal(150);
        currentHealth = maxHealth.GetValue();
    }


    public virtual void DoDamage(CharacterStat targetsStat)
    {
        if (TargetCanAvoidAttack(targetsStat)) return;


        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }
        
        
        totalDamage = CheckTargetArmor(targetsStat, totalDamage);
        targetsStat.TakeDamage(totalDamage);
    }



    public virtual void DoMagicalDamage(CharacterStat targetStat)
    {
        int fireDamageStat = fireDamage.GetValue();
        int iceDamageStat = iceDamage.GetValue();
        int lightingDamageStat = lightingDamage.GetValue();

        int totalMagicalDamage = fireDamageStat + iceDamageStat + lightingDamageStat + intelligence.GetValue();
        totalMagicalDamage = CheckTargetResistance(targetStat, totalMagicalDamage);

        targetStat.TakeDamage(totalMagicalDamage);
    }

    private static int CheckTargetResistance(CharacterStat targetStat, int totalMagicalDamage)
    {
        totalMagicalDamage -= targetStat.magicResistance.GetValue() + targetStat.intelligence.GetValue() * 3;
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    public void ApplyAliments(bool ignite, bool chill, bool shock)
    {
        if (isIgnited || isChilled || isShocked)
        {
            return;
        }

        isIgnited = ignite;
        isChilled = chill;
        isShocked = shock;
    }
    
    
    
    

    private int CheckTargetArmor(CharacterStat targetsStat, int totalDamage)
    {
        totalDamage -= targetsStat.armor.GetValue();

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private bool TargetCanAvoidAttack(CharacterStat targetsStat)
    {
        int totalEvasion = targetsStat.evasion.GetValue() + targetsStat.agility.GetValue();
        if (Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log("Attack avoid");
            return true;
        }

        return false;
    }

    public virtual void TakeDamage(int dam)
    {
        currentHealth -= dam;
        if (currentHealth < 0)
        {
            Die();
        }
    }


    public virtual void Die()
    {
        
    }


    private bool CanCrit()
    {
        int totalCriticalChange = critChance.GetValue() + agility.GetValue();
        if (Random.Range(0, 100) <= totalCriticalChange)
        {
            return true;
        }

        return false;
    }

    private int CalculateCriticalDamage(int dam)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;
        Debug.Log("total crit power %" + totalCritPower);
        float critDamage = dam * totalCritPower;
        Debug.Log("Crit power before round" + critDamage);
        return Mathf.RoundToInt(critDamage);
    }

}
