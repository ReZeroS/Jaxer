using UnityEngine;

public class DeathBringerSpellCastState : DeathBringerState
{


    private float spellTimer;
    private int amountOfSpells;

    
    public DeathBringerSpellCastState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemyDeathBringer curEnemy) : base(stateMachine, baseEnemy, animationName, curEnemy)
    {
    }


    public override void Enter()
    {
        base.Enter();
        spellTimer = enemy.spellCooldown + 0.5f;
        amountOfSpells = enemy.amountOfSpells;
    }

    public override void Update()
    {
        base.Update();
        
        spellTimer -= Time.deltaTime;
        
        if (CanCastSpell())
        {
            enemy.CastSpell();
        }

        if (amountOfSpells <= 0)
        {
            stateMachine.ChangeState(enemy.teleportState);
        }
    }

    private bool CanCastSpell()
    {
        if (amountOfSpells > 0 && spellTimer <= 0)
        {
            spellTimer = enemy.spellCooldown;
            amountOfSpells--;
            return true;
        }
        return false;
    }


    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeCast = Time.time;
    }
}