using UnityEngine;

public class EnemySkeletonBattleState : EnemyState
{
    private Transform playerTransform;
    private EnemySkeleton enemy;
    private int moveToBattleDir;
    
    public EnemySkeletonBattleState(EnemyStateMachine stateMachine, Enemy baseEnemy, EnemySkeleton enemySkeleton, string animationName) : base(stateMachine, baseEnemy, animationName)
    {
        enemy = enemySkeleton;
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

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
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
        
        
        
        if (playerTransform.position.x > enemy.transform.position.x)
        {
            moveToBattleDir = 1;
        } else if (playerTransform.position.x < enemy.transform.position.x)
        {
            moveToBattleDir = -1;
        }
        enemy.SetVelocity(enemy.moveSpeed*moveToBattleDir, rb.linearVelocity.y);
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
