using Sound.SoundManager;
using UnityEngine;

namespace ReZeros.Jaxer.PlayerBase
{
    public class PlayerMoveState : PlayerGroundState
    {
        // vel: 8 
        // 1s 60帧
        // 12 帧 = 0.2s
        // 8  帧 = 0.13s
        // 8/0.2 = 40 
        // 8 / 0.13 = 61.5
        public float accelerationRate = 40f; // 每秒加速量
        public float decelerationRate = 61f; // 每秒减速度
        
        public PlayerMoveState(PlayerStateMachine stateMachine, MainPlayer mainPlayer, string animBoolName)
            : base(stateMachine, mainPlayer, animBoolName)
        {
        }


        public override void Enter()
        {
            base.Enter();
          
            SoundManager.PlaySound(SoundType.MOVEMENT);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
           
            // 切换到闲置状态
            if (Mathf.Abs(xInput) < 0.01f || mainPlayer.IsWallDetected())
            {
                stateMachine.ChangeState(mainPlayer.playerIdleState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (xInput != 0)
            {
                // 加速
                float targetSpeed = xInput * mainPlayer.maxMoveSpeed;
                Vector2 targetVelocity = new Vector2(
                    Mathf.MoveTowards(mainPlayer.GetVelocity().x, targetSpeed,
                        accelerationRate * Time.fixedDeltaTime),
                    mainPlayer.GetVelocity().y
                );
                mainPlayer.SetVelocity(targetVelocity);
            }
            else
            {
                // 减速
                mainPlayer.SetVelocity(new Vector2(
                    Mathf.MoveTowards(mainPlayer.GetVelocity().x, 0, decelerationRate * Time.fixedDeltaTime),
                    mainPlayer.GetVelocity().y
                ));
            }


        }

        public override void Exit()
        {
            base.Exit();
            SoundManager.StopSound();
        }
    }
}