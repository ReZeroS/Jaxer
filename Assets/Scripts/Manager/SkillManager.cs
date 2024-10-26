using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public DashSkill dashSkill { get; private set; }

    public CloneSkill cloneSkill { get; private set; }
    public SwordSkill swordSkill { get; private set; }
    public CrystalSkill crystalSkill { get; private set; }
    public BlackholeSkill blackholeSkill { get; private set; }
    
    public ParrySkill parrySkill { get; private set; }
    
    public DodgeSkill dodgeSkill { get; private set; }


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        dashSkill = GetComponent<DashSkill>();
        cloneSkill = GetComponent<CloneSkill>();
        swordSkill = GetComponent<SwordSkill>();
        crystalSkill = GetComponent<CrystalSkill>();
        blackholeSkill = GetComponent<BlackholeSkill>();
        parrySkill = GetComponent<ParrySkill>();
        dodgeSkill = GetComponent<DodgeSkill>();
    }
    
}