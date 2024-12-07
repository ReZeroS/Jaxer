using ReZeros.Jaxer.Manager;
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
    [SerializeField] private Vector2 launchForce;
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
    private bool isAiming;
    [SerializeField] private float aimSensitivity = 10f;
    public Vector2 aimDirection;
    
    
    
    #region Init skill info
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
    #endregion
    
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
    
    
    public void ThrowSword()
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
        isAiming = false;
        DotsActive(false);
    }
    
    
    protected override void Update()
    {
        base.Update();
        if (!isAiming)
        {
            return;
        }
        
        if (!InputManager.instance.rightTrigger.beingHeld)
        {
            finalDir = new Vector2(aimDirection.normalized.x * launchForce.x,
            aimDirection.normalized.y * launchForce.y);
            // finalDir = new Vector2(15, 15);
            Debug.Log("Sword skill controller set up " + finalDir + " gravity " + swordGravity);
        }

        
        
        // 检测right stick有输入(划定一块死区)
        Vector2 rightStickInput = InputManager.instance.rightStickInput;
        if (rightStickInput.magnitude > 0.1f)
        {
            aimDirection += rightStickInput * (aimSensitivity * Time.deltaTime);
            for (var i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }

   

  


    #region AimRegion
    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (var i = 0; i < dots.Length; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }
    


    private void DotsActive(bool isActive)
    {
        foreach (var t in dots)
        {
            t.SetActive(isActive);
        }
    }

    

    // dest = initPos + vt * (at^2)/2
    private Vector2 DotsPosition(float t)
    {
        return (Vector2)player.transform.position +  // initPos
               aimDirection.normalized * launchForce * t + // vt
               Physics2D.gravity * (0.5f * swordGravity * t * t); // at^2/2
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

    public void AimingSword()
    {
        isAiming = true;
        DotsActive(true);
    }
    
    
    
    
    
    // public Vector2 AnimDirection()
    // {
    //     Vector2 playerPosition = player.transform.position;
    //     Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //     Vector2 direction = mousePosition - playerPosition;
    //     return direction;
    // }
    //
}