public class EnemySkeletonIdleState : EnemySkeletonGroundState
{

    
    public EnemySkeletonIdleState(EnemyStateMachine stateMachine, Enemy baseEnemy, EnemySkeleton enemySkeleton, string animationName) : base(stateMachine, baseEnemy, enemySkeleton, animationName)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        stateTimer = enemySkeleton.idleTime;
    }
    
    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemySkeleton.moveState);
        }
    }


    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.PlaySFX(14, enemySkeleton.transform);
    }


}
