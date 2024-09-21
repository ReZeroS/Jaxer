public class ArcherState : EnemyState
{
    protected EnemyArcher enemyArcher;


    public ArcherState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemyArcher enemyArcher) 
        : base(stateMachine, baseEnemy, animationName)
    {
        this.enemyArcher = enemyArcher;
    }
}
