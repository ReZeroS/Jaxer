public class PlayerGroundState : PlayerState
{
    
    // 定义状态转换检查的优先级顺序
    private readonly StateTransitionCheck[] stateTransitions;

    protected PlayerGroundState(PlayerStateMachine stateMachine, Player player, string animBoolName) 
        : base(stateMachine, player, animBoolName)
    {
        // 按优先级顺序初始化状态转换检查
        stateTransitions = new[]
        {
            new StateTransitionCheck(CheckDropDown, () => player.playerFallingState),
            new StateTransitionCheck(CheckBlackholeSkill, () => player.blackholeState),
            new StateTransitionCheck(CheckSwordSkill, () => player.animSwordState),
            new StateTransitionCheck(CheckCounterAttack, () => player.couterAttackState),
            new StateTransitionCheck(CheckPrimaryAttack, () => player.primaryAttackState),
            new StateTransitionCheck(CheckFalling, () => player.playerFallingState),
            new StateTransitionCheck(CheckJump, () => player.playerJumpState)
        };
    }

    public override void Enter() => base.Enter();

    public override void Exit() => base.Exit();

    public override void Update()
    {
        base.Update();
        
        foreach (var transition in stateTransitions)
        {
            if (transition.TryTransition(stateMachine))
                return;
        }
    }

    #region State Transition Checks
    private bool CheckDropDown()
    {
        if (!ShouldDropDown()) return false;
        
        player.StartDropThroughPlatform();
        return true;
    }

    private bool CheckBlackholeSkill()
    {
        if (!CanUseBlackholeSkill()) return false;
        
        return !player.skillManager.blackholeSkill.IsInCoolDown();
    }

    private bool CheckSwordSkill()
    {
        if (!CanUseSwordSkill()) return false;
        
        return HasNoSword();
    }

    private bool CheckCounterAttack()
    {
        return CanUseCounterAttack();
    }

    private bool CheckPrimaryAttack()
    {
        return InputManager.instance.west.justPressed;
    }

    private bool CheckFalling()
    {
        return !player.IsGroundDetected();
    }

    private bool CheckJump()
    {
        return InputManager.instance.south.justPressed && player.IsGroundDetected();
    }
    #endregion

    #region Condition Checks
    private bool ShouldDropDown()
    {
        return player.canDropDown && 
               InputManager.instance.moveInput.y < 0 && 
               InputManager.instance.south.justPressed;
    }

    private bool CanUseBlackholeSkill()
    {
        return InputManager.instance.padDown.justPressed && 
               player.skillManager.blackholeSkill.blackholeUnlocked;
    }

    private bool CanUseSwordSkill()
    {
        return InputManager.instance.rightTrigger.justPressed && 
               player.skillManager.swordSkill.swordUnlocked;
    }

    private bool CanUseCounterAttack()
    {
        return InputManager.instance.north.beingHeld && 
               player.skillManager.parrySkill.parryUnlocked;
    }

    private bool HasNoSword()
    {
        if (!player.sword) return true;
        
        player.sword.GetComponent<SwordSkillController>().ReturnSword();
        return false;
    }
    #endregion
}
