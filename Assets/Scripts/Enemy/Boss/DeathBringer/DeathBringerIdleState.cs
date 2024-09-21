public class DeathBringerIdleState : DeathBringerState
{
    
    
    public DeathBringerIdleState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemyDeathBringer curEnemy) : base(stateMachine, baseEnemy, animationName, curEnemy)
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
        
        stateMachine.ChangeState(enemy.teleportState);
    }


    public override void Exit()
    {
        base.Exit();
        // AudioManager.instance.PlaySFX(14, enemy.transform);
    }


}