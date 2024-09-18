public class SlimeMoveState : SlimeGroundState
{
   
    

    public SlimeMoveState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemySlime enemySlime) : base(stateMachine, baseEnemy, animationName, enemySlime)
    {
    }
    
    
    
    public override void Enter()
    {
        base.Enter();
    }
    
    public override void Update()
    {
        base.Update();
        enemySlime.SetVelocity(enemySlime.moveSpeed * enemySlime.facingDir, enemySlime.rb.velocity.y);
        if (enemySlime.IsWallDetected() || !enemySlime.IsGroundDetected())
        {
            enemySlime.Flip();
            stateMachine.ChangeState(enemySlime.idleState);
        }
        
    }


    public override void Exit()
    {
        base.Exit();
    }

}
