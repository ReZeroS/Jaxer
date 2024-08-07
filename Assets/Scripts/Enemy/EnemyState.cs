using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnemyState
{
    protected Rigidbody2D rb;
    
    protected EnemyStateMachine stateMachine;

    private Enemy baseEnemy;
    
    

    protected bool triggerCalled;
    private string animationName;
    protected float stateTimer;

    public EnemyState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName)
    {
        this.stateMachine = stateMachine;
        this.baseEnemy = baseEnemy;
        this.animationName = animationName;
    }
    
    
    public virtual void Enter()
    {
        rb = baseEnemy.rb;
        triggerCalled = false;
        baseEnemy.animator.SetBool(animationName, true);
    }
    

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        baseEnemy.animator.SetBool(animationName, false);
    }
    
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
    
    
}
