using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BlackholeSkill : Skill
{
    
    [Header("Blackhole Attack")]
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneAttackCooldown;

    [Header("Blackhole Info")]
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private float blackholeDuration;


    private BlackholeSkillController skillController;

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        GameObject newBlackhole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity);
        skillController = newBlackhole.GetComponent<BlackholeSkillController>();
        skillController.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneAttackCooldown, blackholeDuration);
    }


    public bool SkillCompleted()
    {
        if (!skillController)
        {
            return false;
        }
        if (skillController.playerCanExitState)
        {
            skillController = null;
            return true;
        }
        return false;
    }
    
    
}
