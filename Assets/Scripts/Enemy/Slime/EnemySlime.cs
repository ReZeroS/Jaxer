using UnityEngine;

public enum SlimeType
{
    Small,
    Medium,
    Big
}

public class EnemySlime : Enemy
{
    [Header("Slime specific")]
    [SerializeField] private SlimeType slimeType;

    [SerializeField] private int slimeToCreate;
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private Vector2 minCreateVelocity;
    [SerializeField] private Vector2 maxCreateVelocity;

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

        if (slimeType == SlimeType.Small)
        {
            return;
        }
        CreateSlimes(slimeToCreate, slimePrefab);
    }



    public void CreateSlimes(int amount, GameObject prefab)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject newSlime = Instantiate(prefab, transform.position, Quaternion.identity);
            newSlime.GetComponent<EnemySlime>().SetupSlime(facingDir);
        }
    }


    private void SetupSlime(int faceDir) 
    {
        if (facingDir != faceDir)
        {
            Flip();
        }
        
        
        float xVelocity = Random.Range(minCreateVelocity.x, maxCreateVelocity.x);
        float yVelocity = Random.Range(minCreateVelocity.y, maxCreateVelocity.y);
        
        
        Vector2 velocity = new Vector2(xVelocity * facingDir, yVelocity);
        
        isKnocked = true;
        transform.GetComponent<Rigidbody2D>().linearVelocity = velocity;
        
        Invoke(nameof(CancelKnockback), 1.5f);
    }


    private void CancelKnockback()
    {
        isKnocked = false;
    }
    
    
}