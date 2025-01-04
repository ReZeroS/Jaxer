using System;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        
        if (InputManager.instance.south.justPressed)
        {
            stateMachine.ChangeState(player.playerWallJumpState);
            return;
        }

        
        if (!player.IsWallDetected())
        {
            stateMachine.ChangeState(player.playerFallingState);
            return;
        }


        if (xInput != 0 && Math.Sign(xInput) != Math.Sign(player.facingDir)){
            stateMachine.ChangeState(player.playerIdleState);
        }
        
        if (yInput < 0)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y * 0.7f);
        }

        
        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.playerIdleState);
        }
        
    }
    

    public override void Exit()
    {
        base.Exit();
    }
}
