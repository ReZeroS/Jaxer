using Sound.SoundManager;

public class PlayerSwimState : PlayerOnAirState
{
    public PlayerSwimState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        SoundManager.PlaySound(SoundType.SWIM);
    }

    public override void Update()
    {
        base.Update();
        if (InputManager.instance.south.justPressed && player.IsWaterDetected())
        {
            stateMachine.ChangeState(player.playerJumpState);   
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
