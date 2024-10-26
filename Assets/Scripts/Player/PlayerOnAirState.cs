public class PlayerOnAirState :PlayerState
{
    public PlayerOnAirState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
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
            player.SetVelocity(xInput*player.moveSpeed*0.7f, rb.linearVelocity.y);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
