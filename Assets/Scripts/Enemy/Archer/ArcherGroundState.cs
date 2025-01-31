using ReZeros.Jaxer.Manager;
using UnityEngine;

public class ArcherGroundState : ArcherState
{
 
    
    protected Transform player;
    
    
    public ArcherGroundState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemyArcher enemyArcher) : base(stateMachine, baseEnemy, animationName, enemyArcher)
    {
    }

    
    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.Player.transform;
    }

    public override void Update()
    {
        base.Update();
        if (enemyArcher.IsPlayerDetected() || Vector2.Distance(enemyArcher.transform.position, player.transform.position) < 5)
        {
            stateMachine.ChangeState(enemyArcher.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }


}
