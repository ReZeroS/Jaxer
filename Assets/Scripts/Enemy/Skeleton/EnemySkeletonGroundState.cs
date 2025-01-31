using ReZeros.Jaxer.Manager;
using UnityEngine;

public class EnemySkeletonGroundState : EnemyState
{
    protected EnemySkeleton enemy;

    protected Transform player;
    
    public EnemySkeletonGroundState(EnemyStateMachine stateMachine, Enemy baseEnemy, EnemySkeleton _enemySkeleton, string animationName) : base(stateMachine, baseEnemy, animationName)
    {
        enemy = _enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.Player.transform;
    }

    public override void Update()
    {
        base.Update();
        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.transform.position) < 2)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
