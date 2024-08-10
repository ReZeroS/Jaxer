using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class CloneSkill : Skill
{

    
    [FormerlySerializedAs("gameObject")]
    [Header("Clone info")]
    [SerializeField] private GameObject gb;

    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;
    public void CreateClone(Transform transform)
    {
        GameObject newGameObject = Instantiate(gb);
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
