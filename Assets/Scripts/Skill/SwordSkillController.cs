using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator animator;
    private CircleCollider2D cd;
    private Player player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        cd = GetComponent<CircleCollider2D>();
    }


    public void SetUpSword(Vector2 velocity, float gravity)
    {
        rb.velocity = velocity;
        rb.gravityScale = gravity ;
    }
    
    
    
}
