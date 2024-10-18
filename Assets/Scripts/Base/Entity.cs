using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Entity : MonoBehaviour
{
    #region Component
    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public CharacterStat stat { get; private set; }
    
    public CapsuleCollider2D cd { get; private set; }
    #endregion

    [FormerlySerializedAs("knockedBackpower")]
    [FormerlySerializedAs("knockedDir")]
    [Header("Knocked info")] 
    [SerializeField] protected Vector2 knockedBackPower = new Vector2(7, 12);
    [SerializeField] protected float knockBackDuration = 0.7f;
    [SerializeField] protected Vector2 knockBackOffset = new Vector2(0.5f, 2);
    protected bool isKnocked;
    public int knockBackDir { get; private set; }
    
    [Header("Collision")] 
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance = 1;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance = 0.8f;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected LayerMask whatIsWall;
    public Transform attackCheck;
    public float attackRadius = 1.2f;

    public int facingDir { get; private set; } = 1;
    public bool facingRight { get; private set; } = true;
    
    public event Action onFlipped;


    
    protected virtual void Awake()
    {
       
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stat = GetComponent<CharacterStat>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }


    public virtual void SlowEntityBy(float slowPercentage, float slowDuration)
    {
        
    }


    protected virtual void ReturnDefaultSpeed()
    {
        animator.speed = 1;
    }


    public virtual void DamageImpact()
    {
        StartCoroutine(nameof(HitKnockBack));
    }

    public virtual void SetKnockBackDir(Transform damageDirection)
    {
        if (damageDirection.position.x > transform.position.x)
        {
            knockBackDir = -1;
        } else if (damageDirection.position.x < transform.position.x)
        {
            knockBackDir = 1;
        }
    }
    
    public void SetKnockBackPower(Vector2 power)
    {
        knockedBackPower = power;
    }

    protected virtual IEnumerator HitKnockBack()
    {
        isKnocked = true;
        if (knockedBackPower.x > 0 || knockedBackPower.y > 0) // this line makes freeze effect when get hit
            rb.linearVelocity = new Vector2(knockedBackPower.x * knockBackDir, knockedBackPower.y);
        yield return new WaitForSeconds(knockBackDuration);
        isKnocked = false;
        SetupZeroKnockBackPower();
    }
    
    

    #region Velocity

    public void SetZeroVelocity()
    {
        if (isKnocked)
        {
            return;
        }
        rb.linearVelocity = new Vector2(0, 0);
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnocked)
        {
            return;
        }
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }
    #endregion
    
    
    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsWall);
    
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position,
            new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position,
            new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
        if (attackCheck != null)
        {
            Gizmos.DrawWireSphere(attackCheck.position, attackRadius);
        }
    }
    #endregion
    
    #region Flip
    public virtual void FlipController(float x)
    {
        if (x > 0 && !facingRight)
        {
            Flip();
        } else if (x < 0 && facingRight)
        {
            Flip();
        }
    }
    
    public virtual void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
        onFlipped?.Invoke();
    }
    
    
    
    #endregion



    public virtual void SetupZeroKnockBackPower()
    {
        
    }


    public virtual void Die()
    {
        
    }

}