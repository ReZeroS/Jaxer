using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonMoveState : EnemySkeletonGroundState
{
   

    public EnemySkeletonMoveState(EnemyStateMachine stateMachine, Enemy baseEnemy, EnemySkeleton enemySkeleton, string animationName) : base(stateMachine, baseEnemy, enemySkeleton, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    
    public override void Update()
    {
        base.Update();
        enemySkeleton.SetVelocity(enemySkeleton.moveSpeed * enemySkeleton.facingDir, enemySkeleton.rb.velocity.y);
        if (enemySkeleton.IsWallDetected() || !enemySkeleton.IsGroundDetected())
        {
            enemySkeleton.Flip();
            stateMachine.ChangeState(enemySkeleton.idleState);
        }
        
    }


    public override void Exit()
    {
        base.Exit();
    }

}
