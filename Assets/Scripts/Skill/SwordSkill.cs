using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkill : Skill
{
    [Header("Skill info")] 
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchDir;
    [SerializeField] private float swordGravity;
    private Player player;

    private void Awake()
    {
        player = PlayerManager.instance.player;
    }

    protected override void Update()
    {
        base.Update();
    }

    public void CreateSword()
    {
        GameObject gameObject = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        SwordSkillController skillController = gameObject.GetComponent<SwordSkillController>();
        skillController.SetUpSword(launchDir, swordGravity);
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
