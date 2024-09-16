using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (xInput != 0)
        {
            player.SetVelocity(xInput*player.moveSpeed*0.7f, rb.velocity.y);
        }
        Debug.Log("IsWallDetected" + player.IsWallDetected());
        if (player.IsWallDetected())
        {
            Debug.Log("change to wall slide");
            stateMachine.ChangeState(player.playerWallSlideState);
        }
        
        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.playerIdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
