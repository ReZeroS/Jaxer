using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{

    [Header("Dash")]
    public bool dashUnlocked;
    [SerializeField] private UISkillTreeSlot dashUnlockedButton;
    
    [Header("Clone on dash")]
    public bool cloneOnDashUnlocked;
    [SerializeField] private UISkillTreeSlot cloneOnDashUnlockedButton;

    [Header("Clone on arrived")]
    public bool cloneOnArrivedUnlocked;
    [SerializeField] private UISkillTreeSlot cloneOnArrivedUnlockedButton;

    
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
    
    



}
