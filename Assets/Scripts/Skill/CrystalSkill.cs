using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CrystalSkill : Skill
{

    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefb;
    private GameObject currentCrystal;

    [Header("Explosive crystal")]
    [SerializeField] private bool canExplode;

    [FormerlySerializedAs("canMove")]
    [Header("Moving crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;
    
    
    

    public override void UseSkill()
    {
        base.UseSkill();
        if (currentCrystal == null)
        {
            currentCrystal = Instantiate(crystalPrefb, player.transform.position, Quaternion.identity);
            CrystalSkillController skillController = currentCrystal.GetComponent<CrystalSkillController>();
            skillController.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed,
                FindClosestEnemy(currentCrystal.transform));
        }
        else
        {
            // if crystal can move to enemy then can not use it to teleport
            if (canMoveToEnemy)
            {
                return;
            }
            Vector2 playPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playPos;
            CrystalSkillController skillController = currentCrystal.GetComponent<CrystalSkillController>();
            skillController.FinishedCrystal();
        }
    }
    
    
}
