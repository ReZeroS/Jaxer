namespace ReZeros.Jaxer.PlayerBase
{
    public class PlayerCoyoteTimeState : PlayerOnAirState
    {
        // 应该有个 playerData 替代成员变量

        // float runSpeed = 5f;
        float coyoteTime = 0.1f;

        public PlayerCoyoteTimeState(PlayerStateMachine stateMachine, MainPlayer mainPlayer, string animBoolName)
            : base(stateMachine, mainPlayer, animBoolName)
        {
        }


        public override void Enter()
        {
            base.Enter();
            stateTimer = coyoteTime;
            mainPlayer.SetUseGravity(false);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (InputManager.instance.south.justPressed)
            {
                stateMachine.ChangeState(mainPlayer.playerJumpState);
            }

            if (stateTimer < 0 || xInput != 0)
            {
                stateMachine.ChangeState(mainPlayer.playerFallingState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            mainPlayer.SetUseGravity(true);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}