using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : CharacterStat
{
    private Enemy enemy;
    
    protected override void Start()
    {
        base.Start();
        enemy = GetComponent<Enemy>();
    }

    public override void TakeDamage(int dam)
    {
        base.TakeDamage(dam);
        enemy.DamageEffect();
    }

    public override void Die()
    {
        base.Die();
        enemy.Die();
    }
}
