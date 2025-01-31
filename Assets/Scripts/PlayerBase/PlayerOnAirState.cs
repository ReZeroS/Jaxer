using UnityEngine;

namespace ReZeros.Jaxer.PlayerBase
{
    public class PlayerOnAirState :PlayerState
    {
    
        private bool coyoteTime;

        public PlayerOnAirState(PlayerStateMachine stateMachine, MainPlayer mainPlayer, string animBoolName) : base(stateMachine, mainPlayer, animBoolName)
        {
        }


        public override void Enter()
        {
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            CheckCoyoteTime();

            if (xInput != 0)
            {
                mainPlayer.SetVelocity(xInput*mainPlayer.maxMoveSpeed*0.7f, rb.linearVelocity.y);
            }

            if (InputManager.instance.south.justPressed && mainPlayer.playerJumpState.CanJump())
            {
                stateMachine.ChangeState(mainPlayer.playerJumpState);
            }
        }
    
        private void CheckCoyoteTime()
        {
            if (coyoteTime && Time.time > startTime + mainPlayer.coyoteTime)
            {
                coyoteTime = false;
                mainPlayer.playerJumpState.DecreaseAmountOfJumpsLeft();
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
