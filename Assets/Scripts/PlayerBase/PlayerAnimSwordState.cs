using UnityEngine;

namespace ReZeros.Jaxer.PlayerBase
{
    public class PlayerAnimSwordState : PlayerState
    {
   

        public PlayerAnimSwordState(PlayerStateMachine stateMachine, MainPlayer mainPlayer, string animBoolName) : base(stateMachine, mainPlayer, animBoolName)
        {
        }


        public override void Enter()
        {
            base.Enter();
            mainPlayer.skillManager.swordSkill.AimingSword();
            mainPlayer.SetBusyFor(0.2f);
        }
    

        public override void Exit()
        {
            base.Exit();
            // 退出会进入throwSwordState状态，相当于状态结束直接扔剑
        }
    
    
        public override void LogicUpdate()
        {
            base.LogicUpdate();
        
            mainPlayer.SetZeroVelocity();
        
            // rt released then give up aim direction
            // change means  exi current state means throw sword
            if (!InputManager.instance.rightTrigger.beingHeld)
            {
                stateMachine.ChangeState(mainPlayer.playerIdleState);
            }
        
            // 根据aimDirection 反转方向
            FlipPlayerByAimDirection();
        }

        private void FlipPlayerByAimDirection()
        {
            Vector2 swordSkillAimDirection = SkillManager.instance.swordSkill.aimDirection;
            if (swordSkillAimDirection.x < 0 && mainPlayer.facingRight)
            {
                mainPlayer.Flip();
            } else if (swordSkillAimDirection.x > 0 && !mainPlayer.facingRight)
            {
                mainPlayer.Flip();
            }
        }
    }
}
