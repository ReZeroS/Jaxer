using UnityEngine;

public class PlayerFallingState : PlayerOnAirState
{
    public PlayerFallingState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (player.IsWaterDetected())
        {
            stateMachine.ChangeState(player.playerSwimState);
            return;
        }
        
        if (player.IsWallDetected())
        {
            stateMachine.ChangeState(player.playerWallSlideState);
            return;
        }
        
        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.playerIdleState);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
