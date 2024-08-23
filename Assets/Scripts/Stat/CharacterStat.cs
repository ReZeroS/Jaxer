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

    public Stat critChance;
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


    public bool isIgnited; // does damage over time
    public bool isChilled; // reduce armor by 20% 
    public bool isShocked; // reduce accurancy 


    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    [SerializeField] private float ailmentsDuration = 4f;

    private float igniteDamageCooldown = .3f;
    private float igniteDamageTimer;
    private int igniteDamage;

    [Header("Shock")]
    [SerializeField] private GameObject shockStrikePrefab;

    private int shockDamage;


    #region Event

    public event Action onHealthChanged;

    #endregion


    #region Component

    private EntityFx entityFx;

    #endregion


    public int currentHealth;
    public bool isDead { get; private set; }


    protected virtual void Start()
    {
        critPower.SetDefaultVal(150);
        currentHealth = GetMaxHealthVal();
        entityFx = GetComponent<EntityFx>();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;
        igniteDamageTimer -= Time.deltaTime;
        if (ignitedTimer < 0)
        {
            isIgnited = false;
        }

        if (chilledTimer < 0)
        {
            isChilled = false;
        }

        if (shockedTimer < 0)
        {
            isShocked = false;
        }

        if (isIgnited)
        {
            ApplyIgniteDamage();
        }
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
        
        DoMagicalDamage(targetsStat);// remove if you do not want to do magical damage on primary attack
    }


    #region Magical damage
    public virtual void DoMagicalDamage(CharacterStat targetStat)
    {
        int fireDamageStat = fireDamage.GetValue();
        int iceDamageStat = iceDamage.GetValue();
        int lightingDamageStat = lightingDamage.GetValue();

        int totalMagicalDamage = fireDamageStat + iceDamageStat + lightingDamageStat + intelligence.GetValue();
        totalMagicalDamage = CheckTargetResistance(targetStat, totalMagicalDamage);
        targetStat.TakeDamage(totalMagicalDamage);

        if (Mathf.Max(fireDamageStat, iceDamageStat, lightingDamageStat) <= 0)
        {
            return;
        }

        AttemptToApplyAilment(targetStat, fireDamageStat, iceDamageStat, lightingDamageStat);
    }

    private void AttemptToApplyAilment(CharacterStat targetStat, int fireDamageStat, int iceDamageStat,
        int lightingDamageStat)
    {
        bool canApplyIgnite = fireDamageStat > iceDamageStat && fireDamageStat > lightingDamageStat;
        bool canApplyChill = iceDamageStat > fireDamageStat && iceDamageStat > lightingDamageStat;
        bool canApplyShock = lightingDamageStat > iceDamageStat && lightingDamageStat > fireDamageStat;

        while (!canApplyIgnite && !canApplyIgnite && !canApplyShock)
        {
            if (Random.value < .3f && fireDamageStat > 0)
            {
                canApplyIgnite = true;
                targetStat.ApplyAliments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < .5f && iceDamageStat > 0)
            {
                canApplyChill = true;
                targetStat.ApplyAliments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < .5f && lightingDamageStat > 0)
            {
                canApplyShock = true;
                targetStat.ApplyAliments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }

        if (canApplyIgnite)
        {
            targetStat.SetupIgniteDamage(Mathf.RoundToInt(fireDamageStat * 0.2f));
        }

        if (canApplyShock)
        {
            targetStat.SetupShockDamage(Mathf.RoundToInt(lightingDamageStat * 0.1f));
        }
        
        
        targetStat.ApplyAliments(canApplyIgnite, canApplyChill, canApplyShock);
    }


    public void ApplyAliments(bool ignite, bool chill, bool shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;


        if (ignite && canApplyIgnite)
        {
            isIgnited = ignite;
            ignitedTimer = ailmentsDuration;
            entityFx.IgniteFxFor(ailmentsDuration);
        }

        if (chill && canApplyChill)
        {
            isChilled = chill;
            chilledTimer = ailmentsDuration;
            float slowPer = 0.2f;
            GetComponent<Entity>().SlowEntityBy(slowPer, ailmentsDuration);
            entityFx.ChillFxFor(ailmentsDuration);
        }

        if (shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(shock);
            }
            else
            {
                if (GetComponent<Player>() != null)
                {
                    return;
                }


                HitNearestTargetWithShock();
            }
        }
    }

    public void ApplyShock(bool shock)
    {
        if (isShocked)
        {
            return;
        }
        
        isShocked = shock;
        shockedTimer = ailmentsDuration;
        entityFx.ShockFxFor(ailmentsDuration);
    }

    private void HitNearestTargetWithShock()
    {
        Transform findClosestEnemy = FindUtil.FindClosestEnemyWithoutSelf(transform, 25);
        if (findClosestEnemy)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            newShockStrike.GetComponent<ShockStrikeController>().Setup(
                shockDamage, findClosestEnemy.GetComponent<CharacterStat>()
            );
        }
    }
    
    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0)
        {
            DecreaseHealthBy(igniteDamage);
            if (currentHealth < 0 && !isDead)
            {
                Die();
            }

            igniteDamageTimer = igniteDamageCooldown;
        }
    }


    
    public void SetupIgniteDamage(int dam) => igniteDamage = dam;
    public void SetupShockDamage(int dam) => shockDamage = dam;
    #endregion

    
    public virtual void TakeDamage(int dam)
    {
        DecreaseHealthBy(dam);
        
        GetComponent<Entity>().DamageImpact();
        entityFx.StartCoroutine(nameof(EntityFx.FlashFx));

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }
    
    public virtual void IncreaseHealthBy(int amount)
    {
        currentHealth += amount;
        if (currentHealth > GetMaxHealthVal())
        {
            currentHealth = GetMaxHealthVal();
        }
        onHealthChanged?.Invoke();
    }

    public virtual void IncreaseStatBy(int modifer, float duration, Stat statToModify)
    {
        if (statToModify == null)
        {
            return;
        }
        
        StartCoroutine(StatModCoroutine(modifer, duration, statToModify));

    }

    private IEnumerator StatModCoroutine(int modifer, float duration, Stat statToModify)
    {
        statToModify.AddModifer(modifer);
        yield return new WaitForSeconds(duration);
        statToModify.RemoveModifer(modifer);
    }
    
    

    protected virtual void DecreaseHealthBy(int dam)
    {
        currentHealth -= dam;
        onHealthChanged?.Invoke();
    }


    public virtual void Die()
    {
        isDead = true;
    }

    #region Stat Calculate
    private int CheckTargetArmor(CharacterStat targetsStat, int totalDamage)
    {
        if (targetsStat.isChilled)
        {
            totalDamage -= Mathf.RoundToInt(targetsStat.armor.GetValue() * 0.8f);
        }
        else
        {
            totalDamage -= targetsStat.armor.GetValue();
        }


        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private int CheckTargetResistance(CharacterStat targetStat, int totalMagicalDamage)
    {
        totalMagicalDamage -= targetStat.magicResistance.GetValue() + targetStat.intelligence.GetValue() * 3;
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }
    
    private bool TargetCanAvoidAttack(CharacterStat targetsStat)
    {
        int totalEvasion = targetsStat.evasion.GetValue() + targetsStat.agility.GetValue();
        if (isShocked)
        {
            totalEvasion += 20;
        }

        if (Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log("Attack avoid");
            return true;
        }

        return false;
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

    public int GetMaxHealthVal()
    {
        return maxHealth.GetValue() + agility.GetValue() * 5;
    }
    
    #endregion
}