public class ArcherIdleState : ArcherGroundState
{
    public ArcherIdleState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemyArcher enemyArcher) : base(stateMachine, baseEnemy, animationName, enemyArcher)
    {
    }
    
    
    
    public override void Enter()
    {
        base.Enter();
        stateTimer = enemyArcher.idleTime;
    }
    
    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemyArcher.moveState);
        }
    }


    public override void Exit()
    {
        base.Exit();
        // AudioManager.instance.PlaySFX(14, enemyArcher.transform);
    }
    
}
