using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;

    [SerializeField] private float returnSpeed = 12f;


    public float bounceSpeed;
    public bool isBouncing = true;
    public int amountOfBounce = 4;
    public List<Transform> enemyTargetList;
    public int targetIndex;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        cd = GetComponent<CircleCollider2D>();
    }

    public void SetUpSword(Vector2 velocity, float gravity, Player assignedPlayer)
    {
        player = assignedPlayer;
        rb.velocity = velocity;
        rb.gravityScale = gravity;
        animator.SetBool("Rotate", true);
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

        if (isBouncing && enemyTargetList.Count > 0)
        {
            Vector2 targetPosition = enemyTargetList[targetIndex].position;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, 
                bounceSpeed * Time.deltaTime);

            // 足够靠近敌人时去另一个敌人那
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
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


        StuckIntoCollision(collision);
    }

    private void StuckIntoCollision(Collider2D collision)
    {
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
}