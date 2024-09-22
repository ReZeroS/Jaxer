public class DeathBringerSpellCastState : DeathBringerState
{
    public DeathBringerSpellCastState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemyDeathBringer curEnemy) : base(stateMachine, baseEnemy, animationName, curEnemy)
    {
    }


    public override void Enter()
    {
        base.Enter();
        stateTimer = 5f;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(enemy.teleportState);
        }
    }

}