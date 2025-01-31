using System;
using UnityEngine;

namespace ReZeros.Jaxer.PlayerBase
{
    public class PlayerWallSlideState : PlayerState
    {
        public PlayerWallSlideState(PlayerStateMachine stateMachine, MainPlayer mainPlayer, string animBoolName) : base(stateMachine, mainPlayer, animBoolName)
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
                stateMachine.ChangeState(mainPlayer.playerWallJumpState);
                return;
            }

        
            if (!mainPlayer.IsWallDetected())
            {
                stateMachine.ChangeState(mainPlayer.playerFallingState);
                return;
            }


            if (xInput != 0 && Math.Sign(xInput) != Math.Sign(mainPlayer.facingDir)){
                stateMachine.ChangeState(mainPlayer.playerIdleState);
            }
        
            if (yInput < 0)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
            else
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y * 0.7f);
            }

        
            if (mainPlayer.IsGroundDetected())
            {
                stateMachine.ChangeState(mainPlayer.playerIdleState);
            }
        
        }
    

        public override void Exit()
        {
            base.Exit();
        }
    }
}
