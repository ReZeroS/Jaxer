using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{

    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefb;
    private GameObject currentCrystal;

    [Header("Crystal mirage")]
    [SerializeField] private bool cloneInsteadOfCrystal;
    
    
    [Header("Explosive crystal")]
    [SerializeField] private bool canExplode;

    [Header("Moving crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi stacking crystal")]
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    public List<GameObject> crystalLeft = new();
    
    

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
            FindClosestEnemy(currentCrystal.transform));
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
                    FindClosestEnemy(newCrystal.transform));

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
