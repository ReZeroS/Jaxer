using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class CloneSkill : Skill
{

    
    [FormerlySerializedAs("gameObject")]
    [Header("Clone info")]
    [SerializeField] private GameObject gb;

    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;

    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashOver;
    [SerializeField] private bool createCloneOnCounterAttack;
    
    [Header("Clone can duplicate")]
    [SerializeField] private bool canCreateDuplicateClone;
    [SerializeField] private float duplicateCloneTriggerRate;

    [Header("Crystal instead of clone")]
    public bool crystalInsteadOfClone;
    
    
    public void CreateClone(Transform trans, Vector3 offset)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.instance.crystalSkill.CreateCrystal();
            SkillManager.instance.crystalSkill.CurrentCrystalChooseRandomTarget();
            
            return;
        }
        
        GameObject newGameObject = Instantiate(gb);
        newGameObject.GetComponent<CloneSkillController>().SetupClone(trans, cloneDuration, canAttack, 
            offset, FindClosestEnemy(newGameObject.transform), canCreateDuplicateClone, duplicateCloneTriggerRate, player);
    }



    public void CreateCloneOnDashStart()
    {
        if (createCloneOnDashStart)
        {
            CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnDashOver()
    {
        if (createCloneOnDashOver)
        {
            CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnCounterAttack(Transform enemyTransform)
    {
        if (createCloneOnCounterAttack)
        {
            StartCoroutine(CreateCloneWithDelay(enemyTransform, new Vector3(2 * player.facingDir, 0)));
        }
    }

    private IEnumerator CreateCloneWithDelay(Transform trans, Vector3 offset)
    {
        yield return new WaitForSeconds(0.4f);
        CreateClone(trans, offset);
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
