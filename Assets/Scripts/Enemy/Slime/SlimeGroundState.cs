using ReZeros.Jaxer.Manager;
using UnityEngine;

public class SlimeGroundState : SlimeState
{
   
    protected Transform playerTransform;
    
    public SlimeGroundState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemySlime enemySlime) : base(stateMachine, baseEnemy, animationName, enemySlime)
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
        if (enemySlime.IsPlayerDetected() || Vector2.Distance(enemySlime.transform.position, playerTransform.position) < 2)
        {
            stateMachine.ChangeState(enemySlime.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }


}
