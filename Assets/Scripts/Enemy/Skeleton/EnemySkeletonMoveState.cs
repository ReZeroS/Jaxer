public class EnemySkeletonMoveState : EnemySkeletonGroundState
{
   

    public EnemySkeletonMoveState(EnemyStateMachine stateMachine, Enemy baseEnemy, EnemySkeleton enemySkeleton, string animationName) : base(stateMachine, baseEnemy, enemySkeleton, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    
    public override void Update()
    {
        base.Update();
        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, enemy.rb.linearVelocity.y);
        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
        }
        
    }


    public override void Exit()
    {
        base.Exit();
    }

}
