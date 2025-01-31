using UnityEngine;

namespace ReZeros.Jaxer.PlayerBase
{
    public class PlayerPrimaryAttackState : PlayerState
    {
        private static readonly int ComboCounter = Animator.StringToHash("ComboCounter");

        public int comboCounter { get; private set; }
        private float lastTimeAttacked;
        private int comboWindow = 2;
        public PlayerPrimaryAttackState(PlayerStateMachine stateMachine, MainPlayer mainPlayer, string animBoolName)
            : base(stateMachine, mainPlayer, animBoolName)
        {
        }


        public override void Enter()
        {
            base.Enter();
        
        
            xInput = 0; // fix the attack direction
            if (comboCounter > 2 || lastTimeAttacked + comboWindow < Time.time)
            {
                comboCounter = 0;
            }
            mainPlayer.animator.SetInteger(ComboCounter, comboCounter);

            #region AttackDir
            float attackDir = mainPlayer.facingDir;
            if (xInput != 0)
            {
                attackDir = xInput;
            }
            #endregion
       
        
            mainPlayer.SetVelocity(mainPlayer.attackMovements[comboCounter].x * attackDir, mainPlayer.attackMovements[comboCounter].y);
        
            stateTimer = 0.1f;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (stateTimer < 0)
            {
                rb.linearVelocity = new Vector2(0, 0);
            }
        
            if (animationFinishedTriggerCalled)
            {
                stateMachine.ChangeState(mainPlayer.playerIdleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            comboCounter++;
            lastTimeAttacked = Time.time;
            // for idlestate
            mainPlayer.SetBusyFor(0.1f);
        }
    }
}
