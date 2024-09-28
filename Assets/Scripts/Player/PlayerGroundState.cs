public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (InputManager.instance.padDownJustPressed && player.skillManager.blackholeSkill.blackholeUnlocked)
        {
            if (player.skillManager.blackholeSkill.IsInCoolDown())
            {
                return;
            }
            stateMachine.ChangeState(player.blackholeState);
        }

        
        
        if (InputManager.instance.rightTriggerJustPressed && HasNoSword() && player.skillManager.swordSkill.swordUnlocked)
        {
            stateMachine.ChangeState(player.animSwordState);
        }

        if (InputManager.instance.northBeingHeld && player.skillManager.parrySkill.parryUnlocked)
        {
            stateMachine.ChangeState(player.couterAttackState);
        }
        
        if (InputManager.instance.westJustPressed)
        {
            stateMachine.ChangeState(player.primaryAttackState);
        }
        
        if (!player.IsGroundDetected())
        {
           stateMachine.ChangeState(player.playerAirState); 
        }
        
        if (InputManager.instance.southJustPressed && player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.playerJumpState);
        }
    }

    private bool HasNoSword()
    {
        if (!player.sword)
        {
            return true;
        }
        player.sword.GetComponent<SwordSkillController>().ReturnSword();
        return false;
    }
    
    
    
    public override void Exit()
    {
        base.Exit();
    }
}
