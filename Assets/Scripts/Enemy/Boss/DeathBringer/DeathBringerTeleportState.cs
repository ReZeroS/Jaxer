public class DeathBringerTeleportState : DeathBringerState
{
    public DeathBringerTeleportState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName,
        EnemyDeathBringer curEnemy) : base(stateMachine, baseEnemy, animationName, curEnemy)
    {
    }


    public override void Enter()
    {
        base.Enter();
        enemy.stat.MakeInvulnerable(true);
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            if (enemy.CanSpellCast())
            {
                stateMachine.ChangeState(enemy.spellCastState);
            }
            else
            {
                stateMachine.ChangeState(enemy.battleState);
            } 
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.stat.MakeInvulnerable(false);
    }
}