using UnityEngine;

public class DeathBringerBattleState : DeathBringerState
{
    
    private Transform playerTransform;
    private int moveToBattleDir = 1;
    
    
    public DeathBringerBattleState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemyDeathBringer curEnemy) : base(stateMachine, baseEnemy, animationName, curEnemy)
    {
    }
    
    
    public override void Enter()
    {
        base.Enter();

        Player instancePlayer = PlayerManager.instance.player;
        playerTransform = instancePlayer.transform;
        if (instancePlayer.GetComponent<PlayerStat>().isDead)
        {
            // stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void Update()
    {
        base.Update();

        RaycastHit2D isPlayerDetected = enemy.IsPlayerDetected();
        if (isPlayerDetected)
        {
            stateTimer = enemy.battleTime;
            if (isPlayerDetected.distance < enemy.attackDistance)
            {
                if (CanAttack())
                {
                    stateMachine.ChangeState(enemy.attackState);
                }
            }
        }
        // else boss does need idle
        // {
        //     if (stateTimer < 0 || Vector2.Distance(playerTransform.transform.position, enemy.transform.position) > 15)
        //     {
        //         stateMachine.ChangeState(enemy.idleState);
        //     }
        // }
        
        
        
        if (playerTransform.position.x > enemy.transform.position.x)
        {
            moveToBattleDir = 1;
        } else if (playerTransform.position.x < enemy.transform.position.x)
        {
            moveToBattleDir = -1;
        }
        
        // prevent slime from moving when player is detected and close to attack distance
        if (isPlayerDetected && isPlayerDetected.distance < enemy.attackDistance - 0.1f)
        {
            return;
        }
        
        
        enemy.SetVelocity(enemy.moveSpeed*moveToBattleDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        if (Time.time > enemy.lastTimeAttacked + enemy.attackCoolDown)
        {
            enemy.attackCoolDown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
            enemy.lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }


}