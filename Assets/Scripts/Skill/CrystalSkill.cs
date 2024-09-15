using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalSkill : Skill
{

    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefb;
    private GameObject currentCrystal;

    
    [Header("Crystal simple")]
    [SerializeField] private UISkillTreeSlot unlockCrystalButton;
    public bool crystalUnlocked { get; private set; }

    
    [Header("Crystal mirage")]
    [SerializeField] private UISkillTreeSlot unlockCrystalMirageButton;
    [SerializeField] private bool cloneInsteadOfCrystal;

    [Header("Explosive crystal")]
    [SerializeField] private UISkillTreeSlot unlockCrystalExplosiveButton;
    [SerializeField] private bool canExplode;

    [Header("Moving crystal")]
    [SerializeField] private UISkillTreeSlot unlockCrystalMovingButton;
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi stacking crystal")]
    [SerializeField] private UISkillTreeSlot unlockCrystalMultiStackButton;
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    public List<GameObject> crystalLeft = new();


    protected override void Start()
    {
        base.Start();
        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        unlockCrystalExplosiveButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalExplosive);
        unlockCrystalMirageButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
        unlockCrystalMovingButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMoving);
        unlockCrystalMultiStackButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMultiStack);
    }

    #region Unlock skill
    public void UnlockCrystal()
    {
        if (unlockCrystalButton.unlocked)
        {
            crystalUnlocked = true;
        }
    }


    public void UnlockCrystalExplosive()
    {
        if (unlockCrystalExplosiveButton.unlocked)
        {
            canExplode = true;
        }
    }
    
    public void UnlockCrystalMirage()
    {
        if (unlockCrystalMirageButton.unlocked)
        {
            cloneInsteadOfCrystal = true;
        }
    }
    
    
    public void UnlockCrystalMoving()
    {
        if (unlockCrystalMovingButton.unlocked)
        {
            canMoveToEnemy = true;
        }
    }

    public void UnlockCrystalMultiStack()
    {
        if (unlockCrystalMultiStackButton.unlocked)
        {
            canUseMultiStacks = true;
        }
    }
    #endregion

    

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal())
        {
            return;
        }
        
        if (currentCrystal == null)
        {
            CreateCrystal();
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
            
            if (cloneInsteadOfCrystal)
            {
                SkillManager.instance.cloneSkill.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                CrystalSkillController skillController = currentCrystal.GetComponent<CrystalSkillController>();
                skillController.FinishedCrystal();
            }
            
        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefb, player.transform.position, Quaternion.identity);
        CrystalSkillController controller = currentCrystal.GetComponent<CrystalSkillController>();
        controller.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed,
            FindClosestEnemy(currentCrystal.transform), player);
    }

    public void CurrentCrystalChooseRandomTarget() =>
        currentCrystal.GetComponent<CrystalSkillController>().ChooseRandomEnemy();

    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks)
        {
            if (crystalLeft.Count > 0)
            {
                if (crystalLeft.Count == amountOfStacks)
                {
                    Invoke(nameof(ResetAbility), useTimeWindow);
                }
                
                coolDown = 0;
                // use last one 
                GameObject crystalSpawn = crystalLeft[^1];
                GameObject newCrystal = Instantiate(crystalSpawn, player.transform.position, Quaternion.identity);
                crystalLeft.Remove(crystalSpawn); 
                CrystalSkillController skillController = newCrystal.GetComponent<CrystalSkillController>();
                skillController.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed,
                    FindClosestEnemy(newCrystal.transform), player);

                if (crystalLeft.Count <= 0)
                {
                    // 进冷却
                    coolDown = multiStackCooldown;
                    RefillCrystal();
                }
                return true;
            }
        }
        return false;
    }

    private void RefillCrystal()
    {
        int amountToAdd = amountOfStacks - crystalLeft.Count;
        for (int i = 0; i < amountToAdd; ++i)
        {
            crystalLeft.Add(crystalPrefb);
        }
    }

    private void ResetAbility()
    {
        // 防止重复进冷却
        if (coolDownTimer > 0)
        {
            return;
        }
        coolDownTimer = multiStackCooldown;
        RefillCrystal();
    }
    
}
