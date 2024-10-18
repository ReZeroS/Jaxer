using UnityEngine;

public class SlimeDeadState : SlimeState
{
    
    public SlimeDeadState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName, EnemySlime enemySlime) : base(stateMachine, baseEnemy, animationName, enemySlime)
    {
    }
    
    
    public override void Enter()
    {
        base.Enter();
        enemySlime.animator.SetBool(enemySlime.lastAnimationBoolName, true);
        enemySlime.animator.speed = 0;
        enemySlime.cd.enabled = false;

        stateTimer = 0.1f;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer > 0)
        {
            rb.linearVelocity = new Vector2(0, 10);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}
