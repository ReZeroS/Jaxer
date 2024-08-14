using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;
    
    private static readonly int CounterAttackSuccessful = Animator.StringToHash("CounterAttackSuccessful");

    public PlayerCounterAttackState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        canCreateClone = true;
        stateTimer = player.counterAttackDuration;
        player.animator.SetBool(CounterAttackSuccessful, false);
    }

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();
        
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius);
        foreach (var hit in collider2Ds)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy)
            {
                if (enemy.CanBeStunned())
                {
                    stateTimer = 10f;
                    player.animator.SetBool(CounterAttackSuccessful, true);
                    if (canCreateClone)
                    {
                        canCreateClone = false;         
                        player.skillManager.cloneSkill.CreateCloneOnCounterAttack(hit.transform);
                    }
                }
            }
        }

        if (stateTimer < 0 || animationFinishedTriggerCalled)
        {
            stateMachine.ChangeState(player.playerIdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
