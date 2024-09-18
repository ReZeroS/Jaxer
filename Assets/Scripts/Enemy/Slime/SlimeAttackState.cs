using UnityEngine;

public class SlimeAttackState : SlimeState
{
    public SlimeAttackState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemySlime enemySlime) : base(stateMachine, baseEnemy, animationName, enemySlime)
    {
    }
    
    
    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        enemySlime.SetZeroVelocity();
        
        if (triggerCalled) 
        {
            stateMachine.ChangeState(enemySlime.battleState);
        }
        
    }

    public override void Exit()
    {
        base.Exit();
        enemySlime.lastTimeAttacked = Time.time;
    }
}
