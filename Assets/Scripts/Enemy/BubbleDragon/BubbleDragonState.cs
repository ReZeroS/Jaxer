public class BubbleDragonState : EnemyState
{
    private EnemyBubbleDragon enemy;
    public BubbleDragonState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName) : base(stateMachine, baseEnemy, animationName)
    {
        enemy = (EnemyBubbleDragon)baseEnemy;
    }
    
    
    
}
