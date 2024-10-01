using UnityEngine;

public class BubbleDragonAttackState : BubbleDragonState
{
    public BubbleDragonAttackState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName) : base(stateMachine, baseEnemy, animationName)
    {
    }
    
    
    
    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();
        
        if (triggerCalled) 
        {
            stateMachine.ChangeState(enemy.battleState);
        }
        
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
    }

    
    
}