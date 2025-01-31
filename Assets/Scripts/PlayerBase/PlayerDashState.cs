using Sound.SoundManager;

namespace ReZeros.Jaxer.PlayerBase
{
    public class PlayerDashState : PlayerState
    {

        public PlayerDashState(PlayerStateMachine stateMachine, MainPlayer mainPlayer, string animBoolName) : base(stateMachine, mainPlayer, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            mainPlayer.skillManager.dashSkill.CloneOnDash();
            stateTimer = mainPlayer.dashDuration;
            // todo make invulnerable as a skill
            mainPlayer.stat.MakeInvulnerable(true);
            //
            SoundManager.PlaySound(SoundType.DASH);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            mainPlayer.SetVelocity(mainPlayer.dashSpeed * mainPlayer.dashDir, 0);
        
            if (stateTimer < 0)
            {
                stateMachine.ChangeState(mainPlayer.playerIdleState);
            }
            mainPlayer.fx.CreateAfterImageFx();
        }

        public override void Exit()
        {
            base.Exit();
            mainPlayer.skillManager.dashSkill.CloneOnArrival();
            mainPlayer.SetVelocity(0, rb.linearVelocity.y);
            mainPlayer.stat.MakeInvulnerable(false);
        }
    }
}
