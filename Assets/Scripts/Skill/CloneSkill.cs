using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CloneSkill : Skill
{

    
    [Header("Clone info")]
    [SerializeField] private GameObject gameObject;

    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;
    public void CreateClone(Transform transform)
    {
        GameObject newGameObject = Instantiate(gameObject);
        newGameObject.GetComponent<CloneSkillController>().SetupClone(transform, cloneDuration, canAttack);
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
