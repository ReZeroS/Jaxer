using UnityEngine;

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

        if (Input.GetKeyDown(KeyCode.X) && player.skillManager.blackholeSkill.blackholeUnlocked)
        {
            if (player.skillManager.blackholeSkill.IsInCoolDown())
            {
                return;
            }
            stateMachine.ChangeState(player.blackholeState);
        }
        
        
        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword() && player.skillManager.swordSkill.swordUnlocked)
        {
            stateMachine.ChangeState(player.animSwordState);
        }

        if (Input.GetKeyDown(KeyCode.I) && player.skillManager.parrySkill.parryUnlocked)
        {
            stateMachine.ChangeState(player.couterAttackState);
        }
        
        if (Input.GetKeyDown(KeyCode.J))
        {
            stateMachine.ChangeState(player.primaryAttackState);
        }
        
        if (!player.IsGroundDetected())
        {
           stateMachine.ChangeState(player.playerAirState); 
        }
        
        if (Input.GetKeyDown(KeyCode.K) && player.IsGroundDetected())
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
