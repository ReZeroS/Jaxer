using Sound.SoundManager;

namespace ReZeros.Jaxer.PlayerBase
{
    public class PlayerJumpState : PlayerOnAirState
    {
    
        private static int MAX_JUMPS = 2;

        private int amountOfJumpsLeft;

        public PlayerJumpState(PlayerStateMachine stateMachine, MainPlayer mainPlayer, string animBoolName) : base(stateMachine, mainPlayer, animBoolName)
        {
            amountOfJumpsLeft = MAX_JUMPS;
        }


        public override void Enter()
        {
            base.Enter();
            amountOfJumpsLeft--;
            // 空洞骑士信仰之跃时走这个，不能转向
            // rb.linearVelocity = new Vector2(0, player.jumpForce);
            // 这里后续看看
            mainPlayer.SetVelocity(rb.linearVelocity.x, mainPlayer.jumpForce);
        
            SoundManager.PlaySound(SoundType.JUMP);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (rb.linearVelocity.y < 0)
            {
                stateMachine.ChangeState(mainPlayer.playerFallingState);
            }
        
        }

        public override void Exit()
        {
            base.Exit();
        }
    
        public bool CanJump()
        {
            return amountOfJumpsLeft > 0;
        }

        public void ResetAmountOfJumpsLeft() => amountOfJumpsLeft = MAX_JUMPS;

        public void DecreaseAmountOfJumpsLeft() => amountOfJumpsLeft--;
    }
}
