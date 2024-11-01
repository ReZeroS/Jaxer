using UnityEngine;

public class PlayerState
{


    protected PlayerStateMachine stateMachine;

    protected Player player;

    private string animBoolName;

    protected float xInput;
    protected float yInput;

    protected Rigidbody2D rb;

    protected float startTime;

    protected float stateTimer;

    protected bool animationFinishedTriggerCalled;
    private static readonly int YVelocity = Animator.StringToHash("yVelocity");

    public PlayerState(PlayerStateMachine stateMachine, Player player, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.player = player;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        player.animator.SetBool(animBoolName, true);
        rb = player.rb;
        animationFinishedTriggerCalled = false;
        startTime = Time.time;
    }

    public virtual void LogicUpdate()
    {
        stateTimer -= Time.deltaTime;        
        
        xInput = InputManager.instance.moveInput.x;
        yInput = InputManager.instance.moveInput.y;
        player.animator.SetFloat(YVelocity, rb.linearVelocity.y);
    }

    
    
    
    public virtual void PhysicsUpdate()
    {
    
        
    }
    
    
    public virtual void Exit()
    {
        player.animator.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        animationFinishedTriggerCalled = true;
    }
    
}
