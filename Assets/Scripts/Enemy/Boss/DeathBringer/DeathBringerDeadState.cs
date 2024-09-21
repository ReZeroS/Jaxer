public class DeathBringerDeadState : DeathBringerState
{
    
    public DeathBringerDeadState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemyDeathBringer curEnemy) : base(stateMachine, baseEnemy, animationName, curEnemy)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            // enemy.SelfDestroy();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }


}