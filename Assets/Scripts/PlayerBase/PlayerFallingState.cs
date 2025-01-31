namespace ReZeros.Jaxer.PlayerBase
{
    public class PlayerFallingState : PlayerOnAirState
    {
        public PlayerFallingState(PlayerStateMachine stateMachine, MainPlayer mainPlayer, string animBoolName) : base(stateMachine, mainPlayer, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (mainPlayer.IsWaterDetected())
            {
                stateMachine.ChangeState(mainPlayer.playerSwimState);
                return;
            }
        
            if (mainPlayer.IsWallDetected())
            {
                stateMachine.ChangeState(mainPlayer.playerWallSlideState);
                return;
            }
        
            if (mainPlayer.IsGroundDetected())
            {
                stateMachine.ChangeState(mainPlayer.playerIdleState);
                return;
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
