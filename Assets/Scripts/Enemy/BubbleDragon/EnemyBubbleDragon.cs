using UnityEngine;

public class EnemyBubbleDragon : Enemy
{
    public GameObject bubblePrefab;
    public float bubbleSpeed = 5f;
    
    
    public float bubbleCheckRadius = 2f;
    public float sleepTime = 5f;
    
    public float safeDistance;

    
    public Vector2 jumpVelocity;
    public float jumpCooldown;
    public float lastTimeJumped { get; set; }
    
    
    [Header("Additional collision check")]
    [SerializeField] private Transform groundBehindCheck;
    [SerializeField] private Vector2 groundBehindCheckSize;

    
    
    #region States
    public BubbleDragonIdleState idleState;
    public BubbleDragonMoveState moveState;
    public BubbleDragonJumpState jumpState;
    public BubbleDragonSleepState sleepState;
    public BubbleDragonStunnedState stunnedState;
    public BubbleDragonBattleState battleState;
    public BubbleDragonAttackState attackState;
    public BubbleDragonDeadState deadState;

    #endregion
    
    
    protected override void Awake()
    {
        base.Awake();
        idleState = new BubbleDragonIdleState(stateMachine,this, "Idle");
        moveState = new BubbleDragonMoveState(stateMachine,this, "Move");
        jumpState = new BubbleDragonJumpState(stateMachine,this, "Jump");
        sleepState = new BubbleDragonSleepState(stateMachine,this, "Sleep");
        stunnedState = new BubbleDragonStunnedState(stateMachine,this, "Stunned");
        battleState = new BubbleDragonBattleState(stateMachine,this, "Idle");
        attackState = new BubbleDragonAttackState(stateMachine,this, "Attack");
        deadState = new BubbleDragonDeadState(stateMachine,this, "Dead");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }





    public void ShootBubble()
    {
        GameObject bubbleObj = Instantiate(bubblePrefab, transform.position, transform.rotation);
        BubbleController bubble = bubbleObj.GetComponent<BubbleController>();
    }

    // private IEnumerator CheckForEnemies(BubbleController bubble)
    // {
    //     while (bubble && !bubble.TryCapturingEnemy(null))
    //     {
    //         Collider2D[] colliders = Physics2D.OverlapCircleAll(bubble.transform.position, bubbleCheckRadius);
    //         foreach (Collider2D collider in colliders)
    //         {
    //             if (collider.CompareTag("Player"))
    //             {
    //                 Enemy enemy = collider.GetComponent<Enemy>();
    //                 if (enemy != null && bubble.TryCapturingEnemy(enemy))
    //                 {
    //                     yield break;
    //                 }
    //             }
    //         }
    //
    //         yield return null;
    //     }
    // }
    
    
    
    
    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }


    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    public override void AnimationSpecialTrigger()
    {
        GameObject bubble = Instantiate(bubblePrefab, attackCheck.position, Quaternion.identity);
        bubble.GetComponent<BubbleController>().SetupBubble(bubbleSpeed * facingDir);
    }
    
    
    
    public bool GroundBehindCheck() => Physics2D.BoxCast(groundBehindCheck.position, 
        groundBehindCheckSize, 0, Vector2.zero, whatIsGround);

    public bool WallBehindCheck() => Physics2D.Raycast(wallCheck.position, Vector2.right * -facingDir,
        wallCheckDistance * 2, whatIsWall);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(groundBehindCheck.position, groundBehindCheckSize);
    }
    
}
