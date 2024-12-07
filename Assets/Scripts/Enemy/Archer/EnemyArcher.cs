using UnityEngine;

public class EnemyArcher : Enemy
{

    [Header("Archer specific")] 
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float arrowSpeed = 10;
    public Vector2 jumpVelocity;
    public float jumpCooldown;
    public float sateDistance;
    public float lastTimeJumped { get; set; }
    
    [Header("Additional collision check")]
    [SerializeField] private Transform groundBehindCheck;
    [SerializeField] private Vector2 goundBehindCheckSize;
    
    
    
    #region States
    public ArcherIdleState idleState;
    public ArcherMoveState moveState;
    public ArcherJumpState jumpState;
    public ArcherBattleSate battleState;
    public ArcherAttackState attackState;
    public ArcherDeadState deadState;
    public ArcherStunnedState stunnedState;

    #endregion


    protected override void Awake()
    {
        base.Awake();
        idleState = new ArcherIdleState(stateMachine,this, "Idle", this);
        moveState = new ArcherMoveState(stateMachine,this, "Move", this);
        jumpState = new ArcherJumpState(stateMachine,this, "Jump", this);
        battleState = new ArcherBattleSate(stateMachine,this, "Idle", this);
        attackState = new ArcherAttackState(stateMachine,this, "Attack", this);
        stunnedState = new ArcherStunnedState(stateMachine,this, "Stunned", this);
        deadState = new ArcherDeadState(stateMachine,this, "Dead", this);
        
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

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
        GameObject arrow = Instantiate(arrowPrefab, attackCheck.position, Quaternion.identity);
        arrow.GetComponent<ArrowController>().SetupArrow(arrowSpeed * facingDir, stat);
    }
    
    
    
    public bool GroundBehindCheck() => Physics2D.BoxCast(groundBehindCheck.position, 
        goundBehindCheckSize, 0, Vector2.zero, whatIsGround);

    public bool WallBehindCheck() => Physics2D.Raycast(wallCheck.position, Vector2.right * -facingDir,
            wallCheckDistance * 2, whatIsWall);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(groundBehindCheck.position, goundBehindCheckSize);
    }
}
