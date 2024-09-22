using UnityEngine;

public class DeathBringerAttackState : DeathBringerState
{
    
    public DeathBringerAttackState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemyDeathBringer curEnemy) : base(stateMachine, baseEnemy, animationName, curEnemy)
    {
    }
    
    
    public override void Enter()
    {
        base.Enter();
        enemy.chanceToTeleport += 0.5f;
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();
        
        if (triggerCalled) 
        {
            if (enemy.CanTeleport())
            {
                stateMachine.ChangeState(enemy.teleportState);
            }
            else
            {
                stateMachine.ChangeState(enemy.battleState);
            }
            
        }
        
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
    }


}