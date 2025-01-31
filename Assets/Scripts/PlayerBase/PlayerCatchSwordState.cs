using UnityEngine;

namespace ReZeros.Jaxer.PlayerBase
{
    public class PlayerCatchSwordState : PlayerState
    {

        private Transform sword;
    
        public PlayerCatchSwordState(PlayerStateMachine stateMachine, MainPlayer mainPlayer, string animBoolName) : base(stateMachine, mainPlayer, animBoolName)
        {
        }


        public override void Enter()
        {
            base.Enter();
            sword = mainPlayer.sword.transform;

            DustFxWhenCatchSword();

            // Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // if (mousePosition.x < player.transform.position.x && player.facingRight)
            // {
            //     player.Flip();
            // } else if (mousePosition.x > player.transform.position.x && !player.facingRight)
            // {
            //     player.Flip();
            // }

            rb.linearVelocity = new Vector2(mainPlayer.swordReturnImpact * -mainPlayer.facingDir, rb.linearVelocity.y);

        }

        private void DustFxWhenCatchSword()
        {
            mainPlayer.fx.PlayDustFx();
            mainPlayer.fx.ScreenShakeForSwordImpact(mainPlayer.facingDir);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (animationFinishedTriggerCalled)
            {
                stateMachine.ChangeState(mainPlayer.playerIdleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            mainPlayer.SetBusyFor(0.1f);
        }
    }
}
