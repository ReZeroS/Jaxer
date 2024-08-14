using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private CircleCollider2D cd;
    private Player player;

    
    [Header("Base info")]
    private float returnSpeed;
    private bool canRotate = true;
    private bool isReturning;
    


    [Header("Timer controller")]
    private float freezeTimeDuration;



    [Header("Bounce info")]
    private bool isBouncing;

    private float bounceSpeed;
    private int amountOfBounce;
    private List<Transform> enemyTargetList;
    public int targetIndex;

    [Header("Pierce info")]
    private int pierceAmount;

    [Header("Spin info")]
    private float maxTravelDistance;

    private float spinDuration;
    private float spinTimer;
    private bool spinWasStopped;
    private bool isSpinning;
    private float spinDirection;

    private float hitTimer;
    private float hitCooldown;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        cd = GetComponent<CircleCollider2D>();
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void SetUpSword(Vector2 velocity, float gravity, Player curPlayer, float freezeTimeDelta, float backSpeed)
    {
        rb.velocity = velocity;
        rb.gravityScale = gravity;

        player = curPlayer;
        returnSpeed = backSpeed;
        freezeTimeDuration = freezeTimeDelta;

        if (pierceAmount <= 0)
        {
            animator.SetBool("Rotate", true);
        }

        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

        // too far to return
        // god of war 4 Leviathan Axe 这里后面改成弹射也得弹回来，不用destroy了，控制最大的弹射速度，看重力影响最多多远
        Invoke(nameof(DestroyMe), 7);
    }

    public void SetUpBounce(bool isBounce, float bounceSp, int amountBounce)
    {
        isBouncing = isBounce;
        bounceSpeed = bounceSp;
        amountOfBounce = amountBounce;
        enemyTargetList = new List<Transform>();
    }

    public void SetUpSpin(bool isSpin, float maxDistance, float duration, float cooldown)
    {
        isSpinning = isSpin;
        maxTravelDistance = maxDistance;
        spinDuration = duration;
        hitCooldown = cooldown;
    }


    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        // rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }


    private void Update()
    {
        if (canRotate)
        {
            transform.right = rb.velocity;
        }

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position,
                returnSpeed * Time.deltaTime);

            if (Vector2.Distance(player.transform.position, transform.position) < 1)
            {
                player.CatchSword();
            }
        }

        BounceLogic();

        SpinLogic();
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !spinWasStopped)
            {
                StopWhenSpinning();
            }

            if (spinWasStopped)
            {
                spinTimer -= Time.deltaTime;
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    new Vector2(transform.position.x * spinDirection, transform.position.y),
                    1.5f * Time.deltaTime
                );
                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;
                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;

                    Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, 1);
                    foreach (var hit in collider2Ds)
                    {
                        SwordSkillDamage(hit.GetComponent<Enemy>());
                    }
                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        spinWasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        spinTimer = spinDuration;
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTargetList.Count > 0)
        {
            Vector2 targetPosition = enemyTargetList[targetIndex].position;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition,
                bounceSpeed * Time.deltaTime);

            // 足够靠近敌人时去另一个敌人那
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                SwordSkillDamage(enemyTargetList[targetIndex].GetComponent<Enemy>());

                targetIndex = (targetIndex + 1) % enemyTargetList.Count;
                amountOfBounce--;
                if (amountOfBounce <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
        {
            return;
        }

        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            SwordSkillDamage(enemy);
        }

        SetUpTargetsForBounce(collision);


        StuckIntoCollision(collision);
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        enemy?.DamageEffect();
        enemy?.StartCoroutine("FreezeTimeFor", freezeTimeDuration);
    }

    private void SetUpTargetsForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTargetList.Count <= 0)
            {
                Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, 10);
                foreach (var hit in collider2Ds)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        enemyTargetList.Add(hit.transform);
                    }
                }
            }
        }
    }

    private void StuckIntoCollision(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }


        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTargetList.Count > 0)
        {
            return;
        }

        // 插在敌人身上，跟随敌人
        animator.SetBool("Rotate", false);
        transform.parent = collision.transform;
    }

    public void SetUpPierce(int pierceCount)
    {
        pierceAmount = pierceCount;
    }
}