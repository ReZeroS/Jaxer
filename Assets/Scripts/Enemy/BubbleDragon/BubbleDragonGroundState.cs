using ReZeros.Jaxer.Manager;
using UnityEngine;

public class BubbleDragonGroundState : BubbleDragonState
{
    
    private Transform playerTransform;
    public BubbleDragonGroundState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName) : base(stateMachine, baseEnemy, animationName)
    {
    }
    
    
    public override void Enter()
    {
        base.Enter();
        playerTransform = PlayerManager.instance.Player.transform;
    }

    public override void Update()
    {
        base.Update();
        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, playerTransform.position) < 5)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
    
}