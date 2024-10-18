using UnityEngine;

public class ArcherJumpState : ArcherState
{
    private static readonly int YVelocity = Animator.StringToHash("yVelocity");

    public ArcherJumpState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemyArcher enemyArcher) : base(stateMachine, baseEnemy, animationName, enemyArcher)
    {
    }


    public override void Enter()
    {
        base.Enter();
        // jump back
        enemyArcher.rb.linearVelocity =
            new Vector2(enemyArcher.jumpVelocity.x * -enemyArcher.facingDir, enemyArcher.jumpVelocity.y);
    }

    public override void Update()
    {
        base.Update();
        
        enemyArcher.animator.SetFloat(YVelocity, enemyArcher.rb.linearVelocity.y);
        
        if (rb.linearVelocity.y < 0 && enemyArcher.IsGroundDetected())
        {
            stateMachine.ChangeState(enemyArcher.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}