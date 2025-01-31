using UnityEngine;

namespace ReZeros.Jaxer.PlayerBase
{
    public class PlayerIdleState : PlayerGroundState{
        public PlayerIdleState(PlayerStateMachine stateMachine, MainPlayer mainPlayer, string animBoolName) : base(stateMachine, mainPlayer, animBoolName)
        { 
        
        }


        public override void Enter()
        {
            base.Enter();
            rb.linearVelocity = new Vector2(0, 0);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (Mathf.Sign(xInput) == Mathf.Sign(mainPlayer.facingDir) && mainPlayer.IsWallDetected())
            {
                return;
            }
        
            if (xInput != 0 && !mainPlayer.isBusy)
            {
                stateMachine.ChangeState(mainPlayer.playerMoveState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
