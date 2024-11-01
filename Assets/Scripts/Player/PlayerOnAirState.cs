using UnityEngine;

public class PlayerOnAirState :PlayerState
{
    
    private bool coyoteTime;

    public PlayerOnAirState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }


    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        CheckCoyoteTime();

        if (xInput != 0)
        {
            player.SetVelocity(xInput*player.moveSpeed*0.7f, rb.linearVelocity.y);
        }

        if (InputManager.instance.south.justPressed && player.playerJumpState.CanJump())
        {
            stateMachine.ChangeState(player.playerJumpState);
        }
    }
    
    private void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time > startTime + player.coyoteTime)
        {
            coyoteTime = false;
            player.playerJumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
