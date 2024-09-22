using UnityEngine;

public class EnemyDeathBringer : Enemy
{
    [Header("Teleport Settings")]
    [SerializeField] private BoxCollider2D arena;
    [SerializeField] private Vector2 surroundingCheckSize;
    
    
    [Header("Spell cast settings")]
    [SerializeField] private GameObject spellPrefab;

    private float lastTimeCast;
    [SerializeField] private float spellCastCooldown = 2;
    
    
    
    #region States
    public DeathBringerBattleState battleState { get; private set; }
    public DeathBringerIdleState idleState { get; private set; }
    public DeathBringerAttackState attackState { get; private set; }
    public DeathBringerDeadState deadState { get; private set; }
    public DeathBringerTeleportState teleportState { get; private set; }
    public DeathBringerSpellCastState spellCastState { get; private set; }
    #endregion


    public float chanceToTeleport;
    [SerializeField] private float defaultChanceToTeleport = 25;
    
    
    

    protected override void Awake()
    {
        base.Awake();
        idleState = new DeathBringerIdleState(stateMachine, this, "Idle", this);
        battleState = new DeathBringerBattleState(stateMachine, this, "Move", this);
        attackState = new DeathBringerAttackState(stateMachine, this, "Attack", this);
        deadState = new DeathBringerDeadState(stateMachine, this, "Death", this);
        teleportState = new DeathBringerTeleportState(stateMachine, this, "Teleport", this);
        spellCastState = new DeathBringerSpellCastState(stateMachine, this, "SpellCast", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }
    
    
    
    
    
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }




    public void FindPosition()
    {
        float x = Random.Range(arena.bounds.min.x + 3, arena.bounds.max.x - 3);
        float y = Random.Range(arena.bounds.min.y + 3, arena.bounds.max.y - 3);
        transform.position = new Vector2(x, y);
        transform.position = new Vector2(transform.position.x, transform.position.y - GroundBelow().distance + cd.size.y/2);

        if (!GroundBelow() || SomethingIsGround())
        {
            Debug.Log("No ground found");
            FindPosition();
        }
        
        
    }
    
    
    
    private RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 
        100, whatIsGround);

    private bool SomethingIsGround() => Physics2D.BoxCast(transform.position, surroundingCheckSize, 
        0, Vector2.zero,0, whatIsGround);
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - GroundBelow().distance));
        Gizmos.DrawWireCube(transform.position, surroundingCheckSize);
    }
    
    
    
    public bool CanTeleport()
    {
        float chance = Random.Range(0, 100);
        if (chance <= chanceToTeleport)
        {
            chanceToTeleport = defaultChanceToTeleport;
            return true;
        }
        return false;
    }

    public bool CanSpellCast()
    {
        if (Time.time > lastTimeCast + spellCastCooldown)
        {
            lastTimeCast = Time.time;
            return true;
        }

        return false;
    }
    
    
}
