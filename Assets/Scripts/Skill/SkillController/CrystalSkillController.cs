using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{

    private Animator animator => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();

    private bool canExplode;

    private bool canMove;
    private float moveSpeed;


    private bool canGrow;
    private float growSpeed = 5;
    private Transform closestTraget;
    

    private float crystalExistTimer;
    private static readonly int Explode = Animator.StringToHash("Explode");

    public void SetupCrystal(float crystalDuration, bool canEx, bool canMv, float moveSp,
        Transform findClosestEnemy)
    {
        crystalExistTimer = crystalDuration;
        canExplode = canEx;
        canMove = canMv;
        moveSpeed = moveSp;
        closestTraget = findClosestEnemy;
    }


    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;
        if (crystalExistTimer < 0)
        {
            FinishedCrystal();
        }

        if (canMove)
        {
            transform.position =
                Vector2.MoveTowards(transform.position, closestTraget.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, closestTraget.position) < 2)
            {
                FinishedCrystal();
                // can not move after explode 
                canMove = false;
            }
        }
        
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector3(3, 3), growSpeed * Time.deltaTime);
        }
        
    }

    public void FinishedCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            animator.SetTrigger(Explode);
        }
        else
        {
            SelfDestroy();
        }
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }


    public void CrystalExplodeAnimationEvent()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, cd.radius);
        
        foreach (var hit in collider2Ds)
        {
            Enemy enemy = hit.GetComponent<Enemy>(); 
            enemy?.Damage();
        }
    }


}
