public class BubbleDragonSleepState : BubbleDragonState
{
    public BubbleDragonSleepState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName) : base(stateMachine, baseEnemy, animationName)
    {
    }
    
    
    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.sleepTime;
    }
    
    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }


    public override void Exit()
    {
        base.Exit();
    }

    
}