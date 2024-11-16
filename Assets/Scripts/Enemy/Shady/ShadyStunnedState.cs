using UnityEngine;

public class ShadyStunnedState : ShadyState
{
    public ShadyStunnedState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemyShady enemyShady) : base(stateMachine, baseEnemy, animationName, enemyShady)
    {
    }
    
    
    public override void Enter()
    {
        base.Enter();
        enemy.fx.RedColorBlinkFor(0, .1f);
        
        stateTimer = enemy.stunnedDuration;
        rb.linearVelocity = new Vector2(-enemy.facingDir*enemy.stunnedDirection.x, enemy.stunnedDirection.y);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.fx.CancelColorFor(0);
    }
    
    
}