using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonDeadState : EnemyState
{
    private EnemySkeleton enemySkeleton;

    public EnemySkeletonDeadState(EnemyStateMachine stateMachine, Enemy baseEnemy, EnemySkeleton enemy,
        string animationName)
        : base(stateMachine, baseEnemy, animationName)
    {
        enemySkeleton = enemy;
    }


    public override void Enter()
    {
        base.Enter();
        enemySkeleton.animator.SetBool(enemySkeleton.lastAnimationBoolName, true);
        enemySkeleton.animator.speed = 0;
        enemySkeleton.cd.enabled = false;

        stateTimer = 0.1f;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 10);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}