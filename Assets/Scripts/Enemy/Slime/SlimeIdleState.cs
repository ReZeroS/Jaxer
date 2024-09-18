public class SlimeIdleState : SlimeGroundState
{
    public SlimeIdleState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemySlime enemySlime) : base(stateMachine, baseEnemy, animationName, enemySlime)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        stateTimer = enemySlime.idleTime;
    }
    
    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemySlime.moveState);
        }
    }


    public override void Exit()
    {
        base.Exit();
        // AudioManager.instance.PlaySFX(14, enemySlime.transform);
    }
}
