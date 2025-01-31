using ReZeros.Jaxer.Manager;
using ReZeros.Jaxer.PlayerBase;
using UnityEngine;

public class ShadyBattleState : ShadyState
{
    protected Transform playerTransform;
    private int moveToBattleDir;
    
    private float defaultMoveSpeed;

    public ShadyBattleState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName,
        EnemyShady enemyShady) : base(stateMachine, baseEnemy, animationName, enemyShady)
    {
    }


    public override void Enter()
    {
        base.Enter();

        defaultMoveSpeed = enemy.moveSpeed;
        
        enemy.moveSpeed = enemy.battleMoveSpeed;
        
        MainPlayer instanceMainPlayer = PlayerManager.instance.Player;
        playerTransform = instanceMainPlayer.transform;
        if (instanceMainPlayer.GetComponent<PlayerStat>().isDead)
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
                enemy.stat.KillEntity();
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
        }
        else if (playerTransform.position.x < enemy.transform.position.x)
        {
            moveToBattleDir = -1;
        }

        enemy.SetVelocity(enemy.moveSpeed * moveToBattleDir, rb.linearVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.moveSpeed = defaultMoveSpeed;
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