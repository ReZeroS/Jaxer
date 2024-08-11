using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CloneSkillController : MonoBehaviour
{

    private SpriteRenderer sr;
    private Animator animator;
    [SerializeField] private float loseSpeed;
    private float cloneTimer;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackRadius = .8f;
    private Transform closestEnemy;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void SetupClone(Transform _newTransform, float cloneDuration, bool canAttack, Vector3 offset)
    {
        if (canAttack)
        {
            animator.SetInteger("AttackNumber", Random.Range(1, 4));
        }
        transform.position = _newTransform.position + offset;
        cloneTimer = cloneDuration;
        FaceClosestTarget();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - loseSpeed * Time.deltaTime);
            if (sr.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
    
    
    private void AnimationTrigger()
    {
        cloneTimer = -0.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius);
        foreach (var hit in collider2Ds)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().Damage();
            }
        }
    }

    private void FaceClosestTarget()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, 25);
        float closeDistance = Mathf.Infinity;
        foreach (var hit in collider2Ds)
        {
            if (hit.GetComponent<Enemy>())
            {
                float curDistance = Vector2.Distance(hit.transform.position, transform.position);
                if (curDistance < closeDistance)
                {
                    closeDistance = curDistance;
                    closestEnemy = hit.transform;
                }
            }
        }

        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                transform.Rotate(0, 180, 0);
            }
        }
        
    }
    
    
    
}
