using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonAttackState : EnemyState
{

    private EnemySkeleton enemySkeleton;
    
    public EnemySkeletonAttackState(EnemyStateMachine stateMachine, Enemy baseEnemy, EnemySkeleton _enemySkeleton, string animationName) : base(stateMachine, baseEnemy, animationName)
    {
        enemySkeleton = _enemySkeleton;
    }


    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        enemySkeleton.SetZeroVelocity();
        
        if (triggerCalled) 
        {
            stateMachine.ChangeState(enemySkeleton.battleState);
        }
        
    }

    public override void Exit()
    {
        base.Exit();
        enemySkeleton.lastTimeAttacked = Time.time;
    }
}
