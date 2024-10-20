using UnityEngine;

public class EnemySkeletonAttackState : EnemyState
{

    private EnemySkeleton enemy;
    
    public EnemySkeletonAttackState(EnemyStateMachine stateMachine, Enemy baseEnemy, EnemySkeleton _enemySkeleton, string animationName) : base(stateMachine, baseEnemy, animationName)
    {
        enemy = _enemySkeleton;
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
