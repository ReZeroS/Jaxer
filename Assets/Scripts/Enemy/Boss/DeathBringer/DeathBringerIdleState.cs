using UnityEngine;

public class DeathBringerIdleState : DeathBringerState
{
    
    private Transform playerTransform;
    
    public DeathBringerIdleState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemyDeathBringer curEnemy) : base(stateMachine, baseEnemy, animationName, curEnemy)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
        playerTransform = PlayerManager.instance.player.transform;
    }
    
    public override void Update()
    {
        base.Update();

        if (Vector2.Distance(playerTransform.transform.position, enemy.transform.position) < 7)
        {
            enemy.bossFightBegun = true;
        }


        if (Random.Range(0, 100) < 10)
        {
            stateMachine.ChangeState(enemy.teleportState);
        }


        if (stateTimer <= 0 && enemy.bossFightBegun)
        {
            stateMachine.ChangeState(enemy.battleState);   
        }
    }


    public override void Exit()
    {
        base.Exit();
        // AudioManager.instance.PlaySFX(14, enemy.transform);
    }


}