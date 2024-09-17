using UnityEngine;

public class EnemySkeletonBattleState : EnemyState
{
    private Transform playerTransform;
    private EnemySkeleton enemySkeleton;
    private int moveToBattleDir;
    
    public EnemySkeletonBattleState(EnemyStateMachine stateMachine, Enemy baseEnemy, EnemySkeleton _enemySkeleton, string animationName) : base(stateMachine, baseEnemy, animationName)
    {
        enemySkeleton = _enemySkeleton;
    }
    public override void Enter()
    {
        base.Enter();

        Player instancePlayer = PlayerManager.instance.player;
        playerTransform = instancePlayer.transform;
        if (instancePlayer.GetComponent<PlayerStat>().isDead)
        {
            stateMachine.ChangeState(enemySkeleton.moveState);
        }
    }

    public override void Update()
    {
        base.Update();

        if (enemySkeleton.IsPlayerDetected())
        {
            stateTimer = enemySkeleton.battleTime;
            if (enemySkeleton.IsPlayerDetected().distance < enemySkeleton.attackDistance)
            {
                if (CanAttack())
                {
                    stateMachine.ChangeState(enemySkeleton.attackState);
                }
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(playerTransform.transform.position, enemySkeleton.transform.position) > 15)
            {
                stateMachine.ChangeState(enemySkeleton.idleState);
            }
        }
        
        
        
        if (playerTransform.position.x > enemySkeleton.transform.position.x)
        {
            moveToBattleDir = 1;
        } else if (playerTransform.position.x < enemySkeleton.transform.position.x)
        {
            moveToBattleDir = -1;
        }
        enemySkeleton.SetVelocity(enemySkeleton.moveSpeed*moveToBattleDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        if (Time.time > enemySkeleton.lastTimeAttacked + enemySkeleton.attackCoolDown)
        {
            enemySkeleton.attackCoolDown = Random.Range(enemySkeleton.minAttackCooldown, enemySkeleton.maxAttackCooldown);
            enemySkeleton.lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }

    
}
