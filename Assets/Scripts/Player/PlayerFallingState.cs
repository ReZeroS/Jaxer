public class PlayerFallingState : PlayerOnAirState
{
    public PlayerFallingState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (player.IsWaterDetected())
        {
            stateMachine.ChangeState(player.playerSwimState);
        }
        
        if (player.IsWallDetected())
        {
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
