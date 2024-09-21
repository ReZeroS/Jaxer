using UnityEngine;

public class EnemyShady : Enemy
{
    
    
    [Header("Shady specific")]
    public float battleMoveSpeed = 5f;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float growSpeed = 10f;
    [SerializeField] private float maxSize = 6;
    [SerializeField] private float explosionRadius = 10f;
    
    
    #region States

    public ShadyIdleState idleState { get; private set; }
    public ShadyMoveState moveState { get; private set; }
    public ShadyStunnedState stunnedState { get; private set; }
    public ShadyDeadState deadState { get; private set; }
    public ShadyBattleState battleState { get; private set; }

    #endregion


    protected override void Awake()
    {
        base.Awake();
        idleState = new ShadyIdleState(stateMachine, this, "Idle", this);
        moveState = new ShadyMoveState(stateMachine, this, "Move", this);
        stunnedState = new ShadyStunnedState(stateMachine, this, "Stunned", this);
        deadState = new ShadyDeadState(stateMachine, this, "Dead", this);
        battleState = new ShadyBattleState(stateMachine, this, "MoveFast", this);
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


    public override void AnimationSpecialTrigger()
    {
        base.AnimationSpecialTrigger();
        GameObject explosion = Instantiate(explosionPrefab, attackCheck.position, Quaternion.identity);
        explosion.GetComponent<ExplosiveController>().SetupExplosive(stat, growSpeed, maxSize, explosionRadius);
        cd.enabled = false;
        rb.gravityScale = 0f;
    }


    public void SelfDestroy()
    {
        Destroy(gameObject);
    }

}