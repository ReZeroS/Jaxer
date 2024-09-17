using System;
using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] private LayerMask whatIsPlayer;

    public float playerCheckDistance;
    
    [Header("Move info")] 
    public float moveSpeed;
    public float idleTime;
    public float battleTime;
    private float defaultMoveSpeed;

    [Header("Attack info")] 
    public float attackDistance;
    public float attackCoolDown;
    public float minAttackCooldown;
    public float maxAttackCooldown;
    [HideInInspector] public float lastTimeAttacked;

    [Header("Stunned Info")] 
    public float stunnedDuration;
    public Vector2 stunnedDirection;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;
    
    public EntityFx fx { get; private set; }

    
    public EnemyStateMachine stateMachine { get; private set; }
    public string lastAnimationBoolName { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        defaultMoveSpeed = moveSpeed;
    }

    protected override void Start()
    {
        base.Start();
        fx = GetComponent<EntityFx>();
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    public override void SlowEntityBy(float slowPercentage, float slowDuration)
    {
        moveSpeed *= (1 - slowPercentage);
        animator.speed *= (1 - slowPercentage);
        Invoke(nameof(ReturnDefaultSpeed), slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
    }


    public virtual void FreezeTime(bool freeze)
    {
        if (freeze)
        {
            moveSpeed = 0;
            animator.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            animator.speed = 1;
        }
    }
    
    
    public virtual void FreezeTimeFor(float seconds)
    {
        StartCoroutine(FreezeTimeCoroutine(seconds));
    }

    public virtual IEnumerator FreezeTimeCoroutine(float seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(seconds);
        FreezeTime(false);
    }
    
    

    #region Counter Attack Window
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }
    
    
    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }
    #endregion

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }

        return false;
    }

    
    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, playerCheckDistance,whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }
    
    
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();



    public virtual void AssignLastAnimationName(String lastName)
    {
        lastAnimationBoolName = lastName;
    }
    
    
}
