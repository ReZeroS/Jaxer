using UnityEngine;

public class ArcherAttackState : ArcherState
{
    public ArcherAttackState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName,
        EnemyArcher enemyArcher) : base(stateMachine, baseEnemy, animationName, enemyArcher)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        enemyArcher.SetZeroVelocity();
        
        if (triggerCalled) 
        {
            stateMachine.ChangeState(enemyArcher.battleState);
        }
        
    }

    public override void Exit()
    {
        base.Exit();
        enemyArcher.lastTimeAttacked = Time.time;
    }
    
    
    
}