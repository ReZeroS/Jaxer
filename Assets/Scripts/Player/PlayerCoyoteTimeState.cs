public class PlayerCoyoteTimeState : PlayerOnAirState
{
    // 应该有个 playerData 替代成员变量

    // float runSpeed = 5f;
    float coyoteTime = 0.1f;

    public PlayerCoyoteTimeState(PlayerStateMachine stateMachine, Player player, string animBoolName)
        : base(stateMachine, player, animBoolName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        stateTimer = coyoteTime;
        player.SetUseGravity(false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (InputManager.instance.south.justPressed)
        {
            stateMachine.ChangeState(player.playerJumpState);
        }

        if (stateTimer < 0 || xInput != 0)
        {
            stateMachine.ChangeState(player.playerFallingState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.SetUseGravity(true);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}