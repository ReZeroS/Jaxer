using UnityEngine;

public class SlimeStunnedState : SlimeState
{
    private static readonly int StunFold = Animator.StringToHash("StunFold");

    public SlimeStunnedState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemySlime enemySlime) : base(stateMachine, baseEnemy, animationName, enemySlime)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemySlime.fx.RedColorBlinkFor(0, .1f);
        
        stateTimer = enemySlime.stunnedDuration;
        rb.linearVelocity = new Vector2(-enemySlime.facingDir*enemySlime.stunnedDirection.x, enemySlime.stunnedDirection.y);
    }

    public override void Update()
    {
        base.Update();

        if (rb.linearVelocity.y < 0.1f && enemySlime.IsGroundDetected())
        {
            enemySlime.fx.CancelColorFor(0);
            enemySlime.animator.SetTrigger(StunFold);
            enemySlime.stat.MakeInvulnerable(true);
        }
        

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemySlime.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemySlime.stat.MakeInvulnerable(false);
    }

    
}
