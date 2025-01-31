using Sound.SoundManager;

namespace ReZeros.Jaxer.PlayerBase
{
    public class PlayerSwimState : PlayerOnAirState
    {
        public PlayerSwimState(PlayerStateMachine stateMachine, MainPlayer mainPlayer, string animBoolName) : base(stateMachine, mainPlayer, animBoolName)
        {
        }


        public override void Enter()
        {
            base.Enter();
            SoundManager.PlaySound(SoundType.SWIM);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (InputManager.instance.south.justPressed && mainPlayer.IsWaterDetected())
            {
                stateMachine.ChangeState(mainPlayer.playerJumpState);   
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
