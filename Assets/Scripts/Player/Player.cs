using System.Collections;
using UnityEngine;

public class Player : Entity
{

    [Header("Attack details")] 
    public Vector2[] attackMovements;

    public float counterAttackDuration;
    
    public bool isBusy { get; private set; }
    [Header("Move info")]
    public float moveSpeed = 12f; 
    public float jumpForce = 12f;
    public float swordReturnImpact;
    public float defaultMoveSpeed;
    public float defaultJumpForce;
    public float defaultDashSpeed;
    
    [Header("Dash info")] 
    public float dashSpeed = 12f;
    public float dashDuration = 1.5f;
    public float dashDir { get; private set; }
    

    public SkillManager skillManager { get; private set; }
    public GameObject sword { get; private set; }
    
    public PlayerFx fx { get; private set; }

    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState playerIdleState { get; private set; }
    public PlayerMoveState playerMoveState { get; private set; }
    public PlayerJumpState playerJumpState { get; private set; }
    public PlayerAirState playerAirState { get; private set; }
    
    public PlayerWallSlideState playerWallSlideState { get; private set; }
    public PlayerWallJumpState playerWallJumpState { get; private set; }
    
    public PlayerDashState playerDashState { get; private set; }

    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState couterAttackState { get; private set; }
    
    public PlayerAnimSwordState animSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    
    public PlayerBlackholeState blackholeState { get; private set; }
    public PlayerDeathState deathState { get; private set; }
    
    #endregion

    protected override void Awake()
    { 
        base.Awake();
        stateMachine = new PlayerStateMachine();
        playerIdleState = new PlayerIdleState(stateMachine, this, "Idle");
        playerMoveState = new PlayerMoveState(stateMachine, this, "Move");
        playerJumpState = new PlayerJumpState(stateMachine, this, "Jump");
        playerAirState = new PlayerAirState(stateMachine, this, "Jump");
        playerWallJumpState = new PlayerWallJumpState(stateMachine, this, "Jump");
        playerWallSlideState = new PlayerWallSlideState(stateMachine, this, "WallSlide");
        playerDashState = new PlayerDashState(stateMachine, this, "Dash");
        primaryAttackState = new PlayerPrimaryAttackState(stateMachine, this, "Attack");
        couterAttackState = new PlayerCounterAttackState(stateMachine, this, "CounterAttack");
        animSwordState = new PlayerAnimSwordState(stateMachine, this, "AnimSword");
        catchSwordState = new PlayerCatchSwordState(stateMachine, this, "CatchSword");
        blackholeState = new PlayerBlackholeState(stateMachine, this, "Jump");
        deathState = new PlayerDeathState(stateMachine, this, "Die");
    }


    protected override void Start()
    {
        base.Start();
        fx = GetComponent<PlayerFx>();
        skillManager = SkillManager.instance;
        stateMachine.Initialize(playerIdleState);


        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }

    protected override void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }
        
        base.Update();
        stateMachine.currentState.Update();

        CheckDashInput();

        if (InputManager.instance.padLeft.justPressed && skillManager.crystalSkill.crystalUnlocked)
        {
            skillManager.crystalSkill.CanUseSkill();
        }  
        
        if (InputManager.instance.leftShoulder.justPressed)
        {
            Inventory.instance.UseFlask();
        }

    }


    public override void SlowEntityBy(float slowPercentage, float slowDuration)
    {
        base.SlowEntityBy(slowPercentage, slowDuration);
        moveSpeed *= (1 - slowPercentage);
        jumpForce *= (1 - slowPercentage);
        dashSpeed *= (1 - slowPercentage);
        animator.speed *= (1 - slowPercentage);
        Invoke(nameof(ReturnDefaultSpeed), slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }


    public void AssignSword(GameObject newSword)
    {
        sword = newSword;
    }

    public void CatchSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }


    public void SetBusyFor(float duration)
    {
        StartCoroutine(BusyFor(duration));
    }
    
    private IEnumerator BusyFor(float seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(seconds);
        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    private void CheckDashInput()
    {
        if (IsWallDetected())
        {
            return;
        }

        if (skillManager.dashSkill.dashUnlocked == false)
        {
            return;
        }
        
        if (InputManager.instance.east.justPressed && skillManager.dashSkill.CanUseSkill())
        {
            dashDir = InputManager.instance.moveInput.x;
            if (dashDir == 0)
            {
                dashDir = facingDir;
            }
            stateMachine.ChangeState(playerDashState);
        }
        
        
    }

    public override void SetupZeroKnockBackPower()
    {
        knockedBackPower = new Vector2(0, 0);
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deathState);
    }
    

}
