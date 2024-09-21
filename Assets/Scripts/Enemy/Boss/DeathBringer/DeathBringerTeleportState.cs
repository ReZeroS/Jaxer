public class DeathBringerTeleportState : DeathBringerState
{
    public DeathBringerTeleportState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemyDeathBringer curEnemy) : base(stateMachine, baseEnemy, animationName, curEnemy)
    {
    }


    public override void Enter()
    {
        base.Enter();
        
        enemy.FindPosition();

        stateTimer = 1;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
        
        
    }

    public override void Exit()
    {
        base.Exit();
    }
}