using UnityEngine;

public class PlayerAnimSwordState : PlayerState
{
   

    public PlayerAnimSwordState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        player.skillManager.swordSkill.AimingSword();
        player.SetBusyFor(0.2f);
    }
    

    public override void Exit()
    {
        base.Exit();
        // 退出会进入throwSwordState状态，相当于状态结束直接扔剑
    }
    
    
    public override void Update()
    {
        base.Update();
        
        player.SetZeroVelocity();
        
        // rt released then give up aim direction
        // change means  exi current state means throw sword
        if (!InputManager.instance.rightTriggerBeingHeld)
        {
            stateMachine.ChangeState(player.playerIdleState);
        }
        
        // 根据aimDirection 反转方向
        FlipPlayerByAimDirection();
    }

    private void FlipPlayerByAimDirection()
    {
        Vector2 swordSkillAimDirection = SkillManager.instance.swordSkill.aimDirection;
        Debug.Log("PlayerAnimSwordState swordSkillAimDirection: " + swordSkillAimDirection);
        if (swordSkillAimDirection.x < 0 && player.facingRight)
        {
            player.Flip();
        } else if (swordSkillAimDirection.x > 0 && !player.facingRight)
        {
            player.Flip();
        }
    }
}
