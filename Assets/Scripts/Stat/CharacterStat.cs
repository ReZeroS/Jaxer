using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat : MonoBehaviour
{

    public Stat strength;
    public Stat damage;
    public Stat maxHealth;


    [SerializeField] private int currentHealth;


    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();
    }


    public virtual void DoDamage(CharacterStat targetsStat)
    {
        int totalDamage = damage.GetValue() + strength.GetValue();
        targetsStat.TakeDamage(totalDamage);
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
    

}
