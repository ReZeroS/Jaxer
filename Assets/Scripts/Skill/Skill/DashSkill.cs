using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{

    [Header("Dash")]
    [SerializeField] private UISkillTreeSlot dashUnlockedButton;
    public bool dashUnlocked { get; private set; }

    [Header("Clone on dash")]
    [SerializeField] private UISkillTreeSlot cloneOnDashUnlockedButton;
    public bool cloneOnDashUnlocked { get; private set; }

    [Header("Clone on arrived")]
    [SerializeField] private UISkillTreeSlot cloneOnArrivedUnlockedButton;
    public bool cloneOnArrivedUnlocked { get; private set; }


    public override void CheckUnlock()
    {
        base.CheckUnlock();
        UnlockDash();
        UnlockCloneOnDash();
        UnlockCloneOnArrived();
    }

    public override void UseSkill()
    {
        base.UseSkill();
    }


    protected override void Start()
    {
        base.Start();
        dashUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivedUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrived);
    }

    private void UnlockDash()
    {
        if (dashUnlockedButton.unlocked)
        {
            dashUnlocked = true;
        }
    }



    private void UnlockCloneOnDash()
    {
        if (cloneOnDashUnlockedButton.unlocked)
        {
            cloneOnDashUnlocked = true;
        }
    }

    private void UnlockCloneOnArrived()
    {
        if (cloneOnArrivedUnlockedButton.unlocked)
        {
            cloneOnArrivedUnlocked = true;
        }
    }
    
    public void CloneOnDash()
    {
        if (cloneOnDashUnlocked)
        {
            SkillManager.instance.cloneSkill.CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CloneOnArrival()
    {
        if (cloneOnArrivedUnlocked)
        {
            SkillManager.instance.cloneSkill.CreateClone(player.transform, Vector3.zero);
        }
    }



}
