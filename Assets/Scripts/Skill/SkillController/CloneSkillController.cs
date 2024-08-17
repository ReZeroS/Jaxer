using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CloneSkillController : MonoBehaviour
{

    private Player player;
    private SpriteRenderer sr;
    private Animator animator;
    [SerializeField] private float loseSpeed;
    private float cloneTimer;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackRadius = .8f;
    private Transform closestEnemy;
    private bool canDuplicateClone;
    private float duplicateCloneRate;
    private int facingDir = 1;
    
    
    
    
    private static readonly int AttackNumber = Animator.StringToHash("AttackNumber");

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void SetupClone(Transform newTransform, float cloneDuration, bool canAttack, Vector3 offset,
        Transform findClosestEnemy, bool canCreateDuplicateClone, float duplicateCloneTriggerRate, Player pl)
    {
        if (canAttack)
        {
            animator.SetInteger(AttackNumber, Random.Range(1, 4));
        }
        transform.position = newTransform.position + offset;
        cloneTimer = cloneDuration;
        closestEnemy = findClosestEnemy;
        canDuplicateClone = canCreateDuplicateClone;
        duplicateCloneRate = duplicateCloneTriggerRate;
        player = pl;
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
                player.stat.DoDamage(hit.GetComponent<CharacterStat>());
                if (canDuplicateClone)
                {
                    if (Random.Range(0, 100) < duplicateCloneRate)
                    {
                        SkillManager.instance.cloneSkill.CreateClone(hit.transform , new Vector3(0.5f * facingDir, 0));
                    }
                }
                
                
            }
        }
    }

    private void FaceClosestTarget()
    {
        if (closestEnemy)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }

    
}
