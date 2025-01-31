using UnityEngine;

namespace ReZeros.Jaxer.PlayerBase
{
    public class PlayerBlackholeState : PlayerState
    {

        private float flyTime = 0.4f;
        private bool skillUsed;


        private float defaultGravity;
    
    
        public PlayerBlackholeState(PlayerStateMachine stateMachine, MainPlayer mainPlayer, string animBoolName) : base(stateMachine, mainPlayer, animBoolName)
        {
        }


        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();
        }

        public override void Enter()
        {
            base.Enter();

            defaultGravity = mainPlayer.rb.gravityScale;
        
            skillUsed = false;
            stateTimer = flyTime;
            rb.gravityScale = 0;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (stateTimer > 0)
            {
                rb.linearVelocity = new Vector2(0, 15);
            }

            if (stateTimer < 0)
            {
                rb.linearVelocity = new Vector2(0, -0.1f);
                if (!skillUsed)
                {
                    if (mainPlayer.skillManager.blackholeSkill.CanUseSkill())
                    {
                        skillUsed = true;
                    }
                }
            }

            if (mainPlayer.skillManager.blackholeSkill.SkillCompleted())
            {
                stateMachine.ChangeState(mainPlayer.playerFallingState);
            }
        
        
        }

        public override void Exit()
        {
            base.Exit();
            mainPlayer.rb.gravityScale = defaultGravity;
            mainPlayer.fx.MakeTransparent(false);
        }

    }
}
