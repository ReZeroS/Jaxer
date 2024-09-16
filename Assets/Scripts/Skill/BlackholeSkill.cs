using UnityEngine;
using UnityEngine.UI;

public class BlackholeSkill : Skill
{
    
    [Header("Blackhole Attack")]
    [SerializeField] private UISkillTreeSlot blackholeUnlockButton;
    public bool blackholeUnlocked { get; private set; }
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneAttackCooldown;

    [Header("Blackhole Info")]
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private float blackholeDuration;


    private BlackholeSkillController skillController;


    protected override void Start()
    {
        base.Start();
        blackholeUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlackhole);
    }

    public override void CheckUnlock()
    {
        base.CheckUnlock();
        UnlockBlackhole();
    }

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

    private void UnlockBlackhole()
    {
        if (blackholeUnlockButton.unlocked)
        {
            blackholeUnlocked = true;
        }
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

    public float GetBlackholeRadius()
    {
        return maxSize / 2;
    }
    
    
}
