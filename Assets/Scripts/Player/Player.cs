using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    [Header("Attack details")] 
    public Vector2[] attackMovements;

    public float counterAttackDuration;
    
    public bool isBusy { get; private set; }
    [Header("Move info")]
    [SerializeField] public float moveSpeed = 12f;
    [SerializeField] public float jumpForce = 12f;
    [SerializeField] public float swordReturnImpact;
    [Header("Dash info")] 
    [SerializeField] public float dashSpeed = 12f;
    [SerializeField] public float dashDuration = 1.5f;
    [SerializeField] public float dashDir { get; private set; }




    public SkillManager skillManager { get; private set; }
    public GameObject sword { get; private set; }

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
        skillManager = SkillManager.instance;
        stateMachine.Initialize(playerIdleState);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();

        CheckDashInput();

        if (Input.GetKeyDown(KeyCode.F))
        {
            skillManager.crystalSkill.CanUseSkill();
        }

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
    

    public IEnumerator BusyFor(float seconds)
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
        
        if (Input.GetKeyDown(KeyCode.L) && skillManager.dashSkill.CanUseSkill())
        {
            dashDir = Input.GetAxis("Horizontal");
            if (dashDir == 0)
            {
                dashDir = facingDir;
            }
            stateMachine.ChangeState(playerDashState);
        }
        
        
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deathState);
    }

}
