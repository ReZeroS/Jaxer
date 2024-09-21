public class ArcherDeadState : ArcherState
{
    public ArcherDeadState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemyArcher enemyArcher) : base(stateMachine, baseEnemy, animationName, enemyArcher)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        enemyArcher.animator.SetBool(enemyArcher.lastAnimationBoolName, true);
        enemyArcher.animator.speed = 0;
        // enemyArcher.cd.enabled = false;

        stateTimer = 0.1f;
    }

    public override void Update()
    {
        base.Update();
        // if (stateTimer > 0)
        // {
        //     rb.velocity = new Vector2(0, 10);
        // }
    }

    public override void Exit()
    {
        base.Exit();
    }
    
    
    
    
}