using ReZeros.Jaxer.Manager;
using UnityEngine;

public class ShadyGroundState : ShadyState
{
    protected Transform player;

    
    public ShadyGroundState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemyShady enemyShady) : base(stateMachine, baseEnemy, animationName, enemyShady)
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
        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.transform.position) < 2)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
    
    
}