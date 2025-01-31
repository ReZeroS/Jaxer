using UnityEngine;

namespace ReZeros.Jaxer.PlayerBase
{
    public class PlayerState
    {


        protected PlayerStateMachine stateMachine;

        protected MainPlayer mainPlayer;

        private string animBoolName;

        protected float xInput;
        protected float yInput;

        protected Rigidbody2D rb;

        protected float startTime;

        protected float stateTimer;

        protected bool animationFinishedTriggerCalled;
        private static readonly int YVelocity = Animator.StringToHash("yVelocity");

        public PlayerState(PlayerStateMachine stateMachine, MainPlayer mainPlayer, string animBoolName)
        {
            this.stateMachine = stateMachine;
            this.mainPlayer = mainPlayer;
            this.animBoolName = animBoolName;
        }

        public virtual void Enter()
        {
            mainPlayer.animator.SetBool(animBoolName, true);
            rb = mainPlayer.rb;
            animationFinishedTriggerCalled = false;
            startTime = Time.time;
        }

        public virtual void LogicUpdate()
        {
            stateTimer -= Time.deltaTime;        
        
            xInput = InputManager.instance.moveInput.x;
            yInput = InputManager.instance.moveInput.y;
            mainPlayer.animator.SetFloat(YVelocity, rb.linearVelocity.y);
        }

    
    
    
        public virtual void PhysicsUpdate()
        {
            
        }
    
    
        public virtual void Exit()
        {
            mainPlayer.animator.SetBool(animBoolName, false);
        }

        public virtual void AnimationFinishTrigger()
        {
            animationFinishedTriggerCalled = true;
        }
    
    }
}
