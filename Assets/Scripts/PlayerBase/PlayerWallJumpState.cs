namespace ReZeros.Jaxer.PlayerBase
{
    public class PlayerWallJumpState : PlayerState
    {
        public PlayerWallJumpState(PlayerStateMachine stateMachine, MainPlayer mainPlayer, string animBoolName) : base(stateMachine, mainPlayer, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            stateTimer = 0.4f;
            mainPlayer.SetVelocity(3 * -mainPlayer.facingDir, mainPlayer.wallJumpForce);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (stateTimer < 0)
            {
                stateMachine.ChangeState(mainPlayer.playerFallingState);
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
