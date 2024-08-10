using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class SwordSkill : Skill
{
    [Header("Skill info")] [SerializeField]
    private GameObject swordPrefab;
    
    [SerializeField] private Vector2 launchFoce;
    [SerializeField] private float swordGravity;


    private Vector2 finalDir;

    [Header("Anim dots")] 
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;



    protected override void Start()
    {
        base.Start();
        GenerateDots();
    }
    
    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDir = new Vector2(AnimDirection().normalized.x * launchFoce.x,
                AnimDirection().normalized.y * launchFoce.y);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (var i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
        
    }

    public void CreateSword()
    {
        GameObject createdSword = Instantiate(swordPrefab, player.transform.position,
            transform.rotation);
        SwordSkillController swordController = createdSword.GetComponent<SwordSkillController>(); 
        swordController.SetUpSword(finalDir, swordGravity, player);
        player.AssignSword(createdSword);
        DotsActive(false); 
    }

    public Vector2 AnimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;
        return direction;
    }


    public void DotsActive(bool isActive)
    {
        for (var i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(isActive);
        }
    }
    
    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (var i = 0; i < dots.Length; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    public Vector2 DotsPosition(float t)
    {
        // d = vt * (at^2)/2
        Vector2 postion = (Vector2)player.transform.position + new Vector2(
            AnimDirection().normalized.x * launchFoce.x,
            AnimDirection().normalized.y * launchFoce.y) * t 
            + 0.5f * (Physics2D.gravity * swordGravity) * t * t;
        return postion;
    }


    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
    }
}