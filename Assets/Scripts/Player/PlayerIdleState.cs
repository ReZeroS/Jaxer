using UnityEngine;

public class PlayerIdleState : PlayerGroundState{
    public PlayerIdleState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    { 
        
    }


    public override void Enter()
    {
        base.Enter();
        rb.linearVelocity = new Vector2(0, 0);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Mathf.Sign(xInput) == Mathf.Sign(player.facingDir) && player.IsWallDetected())
        {
            return;
        }
        
        if (xInput != 0 && !player.isBusy)
        {
            stateMachine.ChangeState(player.playerMoveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
