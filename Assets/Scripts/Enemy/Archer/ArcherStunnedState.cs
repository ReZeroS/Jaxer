using UnityEngine;

public class ArcherStunnedState : ArcherState
{
    public ArcherStunnedState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName,
        EnemyArcher enemyArcher) : base(stateMachine, baseEnemy, animationName, enemyArcher)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemyArcher.fx.RedColorBlinkFor(0, .1f);

        stateTimer = enemyArcher.stunnedDuration;
        rb.velocity = new Vector2(-enemyArcher.facingDir * enemyArcher.stunnedDirection.x,
            enemyArcher.stunnedDirection.y);
    }

    public override void Update()
    {
        base.Update();
        
        enemyArcher.fx.CancelColorFor(0);
        enemyArcher.stat.MakeInvulnerable(true);
        
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemyArcher.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemyArcher.stat.MakeInvulnerable(false);
    }
}