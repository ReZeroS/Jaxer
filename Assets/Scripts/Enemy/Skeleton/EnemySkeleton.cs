public class EnemySkeleton : Enemy
{
    #region States

    public EnemySkeletonIdleState idleState { get; private set; }
    public EnemySkeletonMoveState moveState { get; private set; }
    public EnemySkeletonBattleState battleState { get; private set; }
    public EnemySkeletonAttackState attackState { get; private set; }
    public EnemySkeletonStunnedState stunnedState { get; private set; }
    public EnemySkeletonDeadState deadState { get; private set; }

    #endregion


    protected override void Awake()
    {
        base.Awake();
        idleState = new EnemySkeletonIdleState(stateMachine, this, this, "Idle");
        moveState = new EnemySkeletonMoveState(stateMachine, this, this, "Move");
        battleState = new EnemySkeletonBattleState(stateMachine, this, this, "Move");
        attackState = new EnemySkeletonAttackState(stateMachine, this, this, "Attack");
        stunnedState = new EnemySkeletonStunnedState(stateMachine, this, this, "Stunned");
        deadState = new EnemySkeletonDeadState(stateMachine, this, this, "Idle");
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
}