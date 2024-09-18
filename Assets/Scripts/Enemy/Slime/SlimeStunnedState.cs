using UnityEngine;

public class SlimeStunnedState : SlimeState
{
   
    public SlimeStunnedState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemySlime enemySlime) : base(stateMachine, baseEnemy, animationName, enemySlime)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemySlime.fx.RedColorBlinkFor(0, .1f);
        
        stateTimer = enemySlime.stunnedDuration;
        rb.velocity = new Vector2(-enemySlime.facingDir*enemySlime.stunnedDirection.x, enemySlime.stunnedDirection.y);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemySlime.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemySlime.fx.CancelColorFor(0);
    }

    
}
