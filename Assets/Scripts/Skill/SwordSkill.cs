using UnityEngine;
using UnityEngine.UI;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class SwordSkill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Bounce Info")]
    [SerializeField] private UISkillTreeSlot bounceUnlockButton;
    [SerializeField] private int bounceAmount;

    [SerializeField] private float bounceSpeed;
    [SerializeField] private float bounceGravity;


    [Header("Pierce Info")]
    [SerializeField] private UISkillTreeSlot pierceUnlockButton;
    [SerializeField] private int pierceAmount;

    [SerializeField] private float pierceGravity;

    [Header("Spin info")]
    [SerializeField] private UISkillTreeSlot spinUnlockButton;
    [SerializeField] private float maxTravelDistance = 7f;

    [SerializeField] private float spinDuration = 2f;
    [SerializeField] private float spinGravity = 1f;
    [SerializeField] private float hitCooldown = 0.35f;


    [Header("Skill info")]
    [SerializeField] private UISkillTreeSlot swordUnlockButton;

    public bool swordUnlocked { get; private set; }
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchFoce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float returnSpeed;

    [Header("Passive skills")]
    [SerializeField] private UISkillTreeSlot timeStopUnlockButton;
    public bool timeStopUnlocked { get; private set; }
    [SerializeField] private UISkillTreeSlot vulnerableUnlockButton;
    public bool vulnerableUnlocked { get; private set; }

    private Vector2 finalDir;

    [Header("Anim dots")]
    [SerializeField] private int numberOfDots;

    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;


    protected override void Start()
    {
        base.Start();
        GenerateDots();
        SetUpGravity();
        
        
        
        swordUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSword);
        timeStopUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockTimeStop);
        vulnerableUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockVulnerable);
        bounceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBounce);
        pierceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockPierce);
        spinUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSpin);
    }

    private void SetUpGravity()
    {
        if (swordType == SwordType.Bounce)
        {
            swordGravity = bounceGravity;
        }
        else if (swordType == SwordType.Pierce)
        {
            swordGravity = pierceGravity;
        }
        else if (swordType == SwordType.Spin)
        {
            swordGravity = spinGravity;
        }
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDir = new Vector2(AnimDirection().normalized.x * launchFoce.x,
                AnimDirection().normalized.y * launchFoce.y);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (var i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }

    public override void CheckUnlock()
    {
        base.CheckUnlock();
        UnlockSword();
        UnlockTimeStop();
        UnlockVulnerable();
        UnlockBounce();
        UnlockPierce();
        UnlockSpin();   
    }

    public void CreateSword()
    {
        GameObject createdSword = Instantiate(swordPrefab, player.transform.position,
            transform.rotation);
        SwordSkillController swordController = createdSword.GetComponent<SwordSkillController>();

        if (swordType == SwordType.Bounce)
        {
            swordController.SetUpBounce(true, bounceSpeed, bounceAmount);
        }
        else if (swordType == SwordType.Pierce)
        {
            swordController.SetUpPierce(pierceAmount);
        }
        else if (swordType == SwordType.Spin)
        {
            swordController.SetUpSpin(true, maxTravelDistance, spinDuration, hitCooldown);
        }


        swordController.SetUpSword(finalDir, swordGravity, player, freezeTimeDuration, returnSpeed);
        player.AssignSword(createdSword);
        DotsActive(false);
    }
    
    #region UnlockSkills

   


   
    public void UnlockSword()
    {
        if (swordUnlockButton.unlocked)
        {
            swordType = SwordType.Regular;
            swordUnlocked = true;
        }
    }

    public void UnlockTimeStop()
    {
        if (timeStopUnlockButton.unlocked)
        {
            timeStopUnlocked = true;
        }
    }

    public void UnlockVulnerable()
    {
        if (vulnerableUnlockButton.unlocked)
        {
            vulnerableUnlocked = true;
        }
    }

    public void UnlockBounce()
    {
        if (bounceUnlockButton.unlocked)
        {
            swordType = SwordType.Bounce;
        }
    }

    public void UnlockPierce()
    {
        if (pierceUnlockButton.unlocked)
        {
            swordType = SwordType.Pierce;
        }
    }

    public void UnlockSpin()
    {
        if (spinUnlockButton.unlocked)
        {
            swordType = SwordType.Spin;
        }
    }
    
    
    
    #endregion
    
    
    
    
    


    #region AimRegion

    public Vector2 AnimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;
        return direction;
    }


    public void DotsActive(bool isActive)
    {
        for (var i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(isActive);
        }
    }

    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (var i = 0; i < dots.Length; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    public Vector2 DotsPosition(float t)
    {
        // d = vt * (at^2)/2
        Vector2 postion = (Vector2)player.transform.position + new Vector2(
                                                                 AnimDirection().normalized.x * launchFoce.x,
                                                                 AnimDirection().normalized.y * launchFoce.y) * t
                                                             + 0.5f * (Physics2D.gravity * swordGravity) * t * t;
        return postion;
    }

    #endregion

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
    }
}