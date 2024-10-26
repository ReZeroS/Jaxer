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

                    player.skillManager.parrySkill.UseSkill();
                    
                    // Create a clone on counter attack
                    if (canCreateClone)
                    {
                        canCreateClone = false;         
                        player.skillManager.parrySkill.MakeMirageOnParry(hit.transform);
                    }
                }
            }
        }

        if (stateTimer < 0 || animationFinishedTriggerCalled)
        {
            stateMachine.ChangeState(player.playerIdleState);
        }
    }

    private void SuccessfulCounter()
    {
        stateTimer = 10f;
        player.animator.SetBool(CounterAttackSuccessful, true);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
