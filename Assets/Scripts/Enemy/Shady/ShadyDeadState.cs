public class ShadyDeadState : ShadyState
{
    public ShadyDeadState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemyShady enemyShady) : base(stateMachine, baseEnemy, animationName, enemyShady)
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
            enemy.SelfDestroy();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
    
    
    
    
    
    
}