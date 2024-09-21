using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DodgeSkill : Skill
{
    
    [Header("Dodge")]
    [SerializeField] private UISkillTreeSlot unlockDodgeButton;
    public bool dodgeUnlocked { get; private set; }
    [FormerlySerializedAs("evasitonAmount")] [SerializeField] private int evasionAmount;
    
    [Header("Mirage Dodge")]
    [SerializeField] private UISkillTreeSlot unlockMirageDodgeButton;
    public bool mirageDodgeUnlocked { get; private set; }


    protected override void Start()
    {
        base.Start();
        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);
    }

    public override void CheckUnlock()
    {
        base.CheckUnlock();
        UnlockDodge();
        UnlockMirageDodge();
    }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }


    public void UnlockDodge()
    {
        if (unlockDodgeButton.unlocked && !dodgeUnlocked)
        {
            player.stat.evasion.AddModifer(evasionAmount);
            Inventory.instance.UpdateStatSlotsUI();
            dodgeUnlocked = true;
        }
    }
    
    public void UnlockMirageDodge()
    {
        if (unlockMirageDodgeButton.unlocked)
        {
            mirageDodgeUnlocked = true;
        }
    }

    public void CreateMirageDodge()
    {
        if (mirageDodgeUnlocked)
        {
            SkillManager.instance.cloneSkill.CreateClone(player.transform, new Vector3(2 * player.facingDir, 0));
        }
    }
    
    
    
}
