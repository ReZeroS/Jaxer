using UnityEngine;

public class SlimeGroundState : SlimeState
{
   
    protected Transform player;
    
    public SlimeGroundState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemySlime enemySlime) : base(stateMachine, baseEnemy, animationName, enemySlime)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }

    public override void Update()
    {
        base.Update();
        if (enemySlime.IsPlayerDetected() || Vector2.Distance(enemySlime.transform.position, player.transform.position) < 2)
        {
            stateMachine.ChangeState(enemySlime.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }


}
