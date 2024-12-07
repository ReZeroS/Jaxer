using ReZeros.Jaxer.Manager;
using UnityEngine;

public class SlimeBattleState : SlimeState
{
    private Transform playerTransform;
    private int moveToBattleDir;

    public SlimeBattleState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemySlime enemySlime) : base(stateMachine, baseEnemy, animationName, enemySlime)
    {
    }
    
    
    public override void Enter()
    {
        base.Enter();

        Player instancePlayer = PlayerManager.instance.player;
        playerTransform = instancePlayer.transform;
        if (instancePlayer.GetComponent<PlayerStat>().isDead)
        {
            stateMachine.ChangeState(enemySlime.moveState);
        }
    }

    public override void Update()
    {
        base.Update();
        RaycastHit2D isPlayerDetected = enemySlime.IsPlayerDetected();
        if (isPlayerDetected)
        {
            stateTimer = enemySlime.battleTime;
            if (isPlayerDetected.distance < enemySlime.attackDistance)
            {
                if (CanAttack())
                {
                    stateMachine.ChangeState(enemySlime.attackState);
                }
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(playerTransform.transform.position, enemySlime.transform.position) > 15)
            {
                stateMachine.ChangeState(enemySlime.idleState);
            }
        }
        
        
        
        if (playerTransform.position.x > enemySlime.transform.position.x)
        {
            moveToBattleDir = 1;
        } else if (playerTransform.position.x < enemySlime.transform.position.x)
        {
            moveToBattleDir = -1;
        }

        // prevent slime from moving when player is detected and close to attack distance
        if (isPlayerDetected && isPlayerDetected.distance < enemySlime.attackDistance - 0.1f)
        {
            return;
        }
        
        enemySlime.SetVelocity(enemySlime.moveSpeed*moveToBattleDir, rb.linearVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        if (Time.time > enemySlime.lastTimeAttacked + enemySlime.attackCoolDown)
        {
            enemySlime.attackCoolDown = Random.Range(enemySlime.minAttackCooldown, enemySlime.maxAttackCooldown);
            enemySlime.lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }


    
    
}
