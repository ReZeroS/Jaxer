public class ShadyState : EnemyState
{
    protected EnemyShady enemy;
    public ShadyState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemyShady enemyShady) : base(stateMachine, baseEnemy, animationName)
    {
        enemy = enemyShady;
    }
    
    
    
}