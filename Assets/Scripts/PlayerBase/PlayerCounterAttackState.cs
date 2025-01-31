using UnityEngine;

namespace ReZeros.Jaxer.PlayerBase
{
    public class PlayerCounterAttackState : PlayerState
    {
        private bool canCreateClone;
    
        private static readonly int CounterAttackSuccessful = Animator.StringToHash("CounterAttackSuccessful");

        public PlayerCounterAttackState(PlayerStateMachine stateMachine, MainPlayer mainPlayer, string animBoolName) : base(stateMachine, mainPlayer, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            canCreateClone = true;
            stateTimer = mainPlayer.counterAttackDuration;
            mainPlayer.animator.SetBool(CounterAttackSuccessful, false);
        
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            mainPlayer.SetZeroVelocity();
        
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(mainPlayer.attackCheck.position, mainPlayer.attackRadius);
            foreach (var hit in collider2Ds)
            {
                ArrowController arrowController = hit.GetComponent<ArrowController>();
                if (arrowController)
                {
                    arrowController.Flip();
                    SuccessfulCounter();
                }


                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy)
                {
                    if (enemy.CanBeStunned())
                    {
                        SuccessfulCounter();

                        mainPlayer.skillManager.parrySkill.UseSkill();
                    
                        // Create a clone on counter attack
                        if (canCreateClone)
                        {
                            canCreateClone = false;         
                            mainPlayer.skillManager.parrySkill.MakeMirageOnParry(hit.transform);
                        }
                    }
                }
            }

            if (stateTimer < 0 || animationFinishedTriggerCalled)
            {
                stateMachine.ChangeState(mainPlayer.playerIdleState);
            }
        }

        private void SuccessfulCounter()
        {
            stateTimer = 10f;
            mainPlayer.animator.SetBool(CounterAttackSuccessful, true);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
