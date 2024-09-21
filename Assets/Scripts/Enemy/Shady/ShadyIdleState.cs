public class ShadyIdleState : ShadyGroundState
{
    public ShadyIdleState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemyShady enemyShady) : base(stateMachine, baseEnemy, animationName, enemyShady)
    {
    }
    
    
    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
    }
    
    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }


    public override void Exit()
    {
        base.Exit();
        // AudioManager.instance.PlaySFX(14, enemy.transform);
    }

    
    
}