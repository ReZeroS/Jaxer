using UnityEngine;

public class EnemySkeletonGroundState : EnemyState
{
    protected EnemySkeleton enemySkeleton;

    protected Transform player;
    
    public EnemySkeletonGroundState(EnemyStateMachine stateMachine, Enemy baseEnemy, EnemySkeleton _enemySkeleton, string animationName) : base(stateMachine, baseEnemy, animationName)
    {
        enemySkeleton = _enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }

    public override void Update()
    {
        base.Update();
        if (enemySkeleton.IsPlayerDetected() || Vector2.Distance(enemySkeleton.transform.position, player.transform.position) < 2)
        {
            stateMachine.ChangeState(enemySkeleton.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
