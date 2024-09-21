public class DeathBringerState : EnemyState
{
    
    protected EnemyDeathBringer enemy;
    
    public DeathBringerState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemyDeathBringer curEnemy) : base(stateMachine, baseEnemy, animationName)
    {
        enemy = curEnemy;
    }
}