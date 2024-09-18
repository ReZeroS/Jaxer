public class EnemySlime : Enemy
{
    
    #region States

    public SlimeIdleState idleState { get; private set; }
    public SlimeMoveState moveState { get; private set; }
    public SlimeBattleState battleState { get; private set; }
    public SlimeAttackState attackState { get; private set; }
    public SlimeStunnedState stunnedState { get; private set; }
    public SlimeDeadState deadState { get; private set; }

    #endregion


    protected override void Awake()
    {
        base.Awake();
        idleState = new SlimeIdleState(stateMachine, this, "Idle", this);
        moveState = new SlimeMoveState(stateMachine, this, "Move", this);
        battleState = new SlimeBattleState(stateMachine, this, "Move", this);
        attackState = new SlimeAttackState(stateMachine, this, "Attack", this);
        stunnedState = new SlimeStunnedState(stateMachine, this, "Stun", this);
        deadState = new SlimeDeadState(stateMachine, this, "Idle", this);
    }


    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
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
