using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CloneSkill : Skill
{

    
    [FormerlySerializedAs("gameObject")]
    [Header("Clone info")]
    [SerializeField] private float attackMultiplier;
    [SerializeField] private GameObject gb;
    [SerializeField] private float cloneDuration;


    [Header("Clone attack")]
    [SerializeField] private UISkillTreeSlot cloneAttackUnlockButton;
    [SerializeField] private float cloneAttackMultiplier;
    [SerializeField] private bool canAttack;
    
    [Header("Aggresive clone")]
    [SerializeField] private UISkillTreeSlot aggresiveCloneUnlockButton;
    [SerializeField] private float aggresiveCloneAttackMultiplier;
    public bool canApplyOnHitEffect { get; private set; }
    
    [Header("Multiple clone")]
    [SerializeField] private UISkillTreeSlot multipleCloneUnlockButton;
    [SerializeField] private float multipleCloneAttackMultiplier;
    [SerializeField] private bool canCreateDuplicateClone;
    [SerializeField] private float duplicateCloneTriggerRate;

    [Header("Crystal instead of clone")]
    [SerializeField] private UISkillTreeSlot crystalInsteadOfCloneUnlockButton;
    public bool crystalInsteadOfClone;


    protected override void Start()
    {
        base.Start();
        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggresiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggresiveClone);
        multipleCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultipleClone);
        crystalInsteadOfCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInsteadOfClone);
    }

    #region Unlock skill

    public void UnlockCloneAttack()
    {
        if (cloneAttackUnlockButton.unlocked)
        {
            canAttack = true;
            attackMultiplier = cloneAttackMultiplier;
        }
    }

    public void UnlockAggresiveClone()
    {
        if (aggresiveCloneUnlockButton.unlocked)
        {
            canApplyOnHitEffect = true;
            attackMultiplier = aggresiveCloneAttackMultiplier;
        }
    }

    public void UnlockMultipleClone()
    {
        if (multipleCloneUnlockButton.unlocked)
        {
            canCreateDuplicateClone = true;
            attackMultiplier = multipleCloneAttackMultiplier;
        }
    }

    public void UnlockCrystalInsteadOfClone()
    {
        if (crystalInsteadOfCloneUnlockButton.unlocked)
        {
            crystalInsteadOfClone = true;
        }
    }

    #endregion
    
    
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
            offset, FindClosestEnemy(newGameObject.transform), canCreateDuplicateClone, duplicateCloneTriggerRate, player, attackMultiplier);
    }

 
 
   

    public void CreateCloneWithDelay(Transform enemyTransform)
    {
        StartCoroutine(CreateCloneWithDelayCoroutine(enemyTransform, new Vector3(2 * player.facingDir, 0)));
    }

    private IEnumerator CreateCloneWithDelayCoroutine(Transform trans, Vector3 offset)
    {
        yield return new WaitForSeconds(0.4f);
        CreateClone(trans, offset);
    }


    public override void CheckUnlock()
    {
        base.CheckUnlock();
        UnlockCloneAttack();
        UnlockAggresiveClone();
        UnlockMultipleClone();
        UnlockCrystalInsteadOfClone();
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
