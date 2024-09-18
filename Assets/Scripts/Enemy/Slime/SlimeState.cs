public class SlimeState : EnemyState
{
    
    protected EnemySlime enemySlime;

    public SlimeState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemySlime enemySlime) : base(stateMachine, baseEnemy, animationName)
    {
        this.enemySlime = enemySlime;
    }
}
