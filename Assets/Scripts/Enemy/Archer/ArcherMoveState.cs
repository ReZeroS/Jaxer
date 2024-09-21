public class ArcherMoveState : ArcherGroundState
{
    public ArcherMoveState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemyArcher enemyArcher) : base(stateMachine, baseEnemy, animationName, enemyArcher)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        enemyArcher.SetVelocity(enemyArcher.moveSpeed * enemyArcher.facingDir, enemyArcher.rb.velocity.y);
        if (enemyArcher.IsWallDetected() || !enemyArcher.IsGroundDetected())
        {
            enemyArcher.Flip();
            stateMachine.ChangeState(enemyArcher.idleState);
        }
        
    }


    public override void Exit()
    {
        base.Exit();
    }

   
}