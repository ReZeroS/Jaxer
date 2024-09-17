using UnityEngine;

public class EnemySkeletonStunnedState : EnemyState
{
    private EnemySkeleton enemySkeleton;
    public EnemySkeletonStunnedState(EnemyStateMachine stateMachine, Enemy baseEnemy, EnemySkeleton _enemySkeleton, string animationName) : base(stateMachine, baseEnemy, animationName)
    {
        enemySkeleton = _enemySkeleton;
    }


    public override void Enter()
    {
        base.Enter();
        enemySkeleton.fx.RedColorBlinkFor(0, .1f);
        
        stateTimer = enemySkeleton.stunnedDuration;
        rb.velocity = new Vector2(-enemySkeleton.facingDir*enemySkeleton.stunnedDirection.x, enemySkeleton.stunnedDirection.y);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemySkeleton.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemySkeleton.fx.CancelColorFor(0);
    }
}
