using UnityEngine;

public class BubbleDragonBattleState : BubbleDragonState
{
    
    protected Transform playerTransform;
    public BubbleDragonBattleState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName) : base(stateMachine, baseEnemy, animationName)
    {
    }
    
    
    
    
    public override void Enter()
    {
        base.Enter();

        Player instancePlayer = PlayerManager.instance.player;
        playerTransform = instancePlayer.transform;
        if (instancePlayer.GetComponent<PlayerStat>().isDead)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void Update()
    {
        base.Update();
        RaycastHit2D isPlayerDetected = enemy.IsPlayerDetected();
        if (isPlayerDetected)
        {
            stateTimer = enemy.battleTime;

            if (isPlayerDetected.distance < enemy.safeDistance)
            {
                if (CanJump())
                {
                    stateMachine.ChangeState(enemy.jumpState);
                }
            }
            
            if (isPlayerDetected.distance < enemy.attackDistance)
            {
                if (CanAttack())
                {
                    stateMachine.ChangeState(enemy.attackState);
                }
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(playerTransform.transform.position, enemy.transform.position) > 15)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }
        
        
        BattleStateFlipController();
    }

    private void BattleStateFlipController()
    {
        if (playerTransform.position.x > enemy.transform.position.x && enemy.facingDir == -1)
        {
            enemy.Flip();
        } else if (playerTransform.position.x < enemy.transform.position.x && enemy.facingDir == 1)
        {
            enemy.Flip();
        }
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


    
    
    private bool CanJump()
    {
        if (!enemy.GroundBehindCheck() || enemy.WallBehindCheck())
        {
            return false;
        }
        
        if (Time.time >= enemy.lastTimeJumped + enemy.jumpCooldown)
        {
            enemy.lastTimeJumped = Time.time;
            return true;
        }
        
        return false;
    }

    
    
}