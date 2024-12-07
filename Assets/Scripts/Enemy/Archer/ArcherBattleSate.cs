using ReZeros.Jaxer.Manager;
using UnityEngine;

public class ArcherBattleSate : ArcherState
{
    
    private Transform playerTransform;
    private int moveToBattleDir;
    public ArcherBattleSate(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemyArcher enemyArcher) : base(stateMachine, baseEnemy, animationName, enemyArcher)
    {
    }
    
     
    
    public override void Enter()
    {
        base.Enter();

        Player instancePlayer = PlayerManager.instance.player;
        playerTransform = instancePlayer.transform;
        if (instancePlayer.GetComponent<PlayerStat>().isDead)
        {
            stateMachine.ChangeState(enemyArcher.moveState);
        }
    }

    public override void Update()
    {
        base.Update();
        RaycastHit2D isPlayerDetected = enemyArcher.IsPlayerDetected();
        if (isPlayerDetected)
        {
            stateTimer = enemyArcher.battleTime;

            if (isPlayerDetected.distance < enemyArcher.sateDistance)
            {
                if (CanJump())
                {
                    stateMachine.ChangeState(enemyArcher.jumpState);
                }
            }
            
            if (isPlayerDetected.distance < enemyArcher.attackDistance)
            {
                if (CanAttack())
                {
                    stateMachine.ChangeState(enemyArcher.attackState);
                }
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(playerTransform.transform.position, enemyArcher.transform.position) > 15)
            {
                stateMachine.ChangeState(enemyArcher.idleState);
            }
        }
        
        
        BattleStateFlipController();
    }

    private void BattleStateFlipController()
    {
        if (playerTransform.position.x > enemyArcher.transform.position.x && enemyArcher.facingDir == -1)
        {
            enemyArcher.Flip();
        } else if (playerTransform.position.x < enemyArcher.transform.position.x && enemyArcher.facingDir == 1)
        {
            enemyArcher.Flip();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        if (Time.time > enemyArcher.lastTimeAttacked + enemyArcher.attackCoolDown)
        {
            enemyArcher.attackCoolDown = Random.Range(enemyArcher.minAttackCooldown, enemyArcher.maxAttackCooldown);
            enemyArcher.lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }


    
    
    private bool CanJump()
    {
        bool groundBehindCheck = enemyArcher.GroundBehindCheck();
        if (!groundBehindCheck || enemyArcher.WallBehindCheck())
        {
            return false;
        }
        
        if (Time.time >= enemyArcher.lastTimeJumped + enemyArcher.jumpCooldown)
        {
            enemyArcher.lastTimeJumped = Time.time;
            return true;
        }
        
        return false;
    }


    
    
    
}
