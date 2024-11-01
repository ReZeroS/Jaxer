using Sound.SoundManager;

public class PlayerDashState : PlayerState
{

    public PlayerDashState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.skillManager.dashSkill.CloneOnDash();
        stateTimer = player.dashDuration;
        // todo make invulnerable as a skill
        player.stat.MakeInvulnerable(true);
        //
        SoundManager.PlaySound(SoundType.DASH);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        player.SetVelocity(player.dashSpeed * player.dashDir, 0);
        
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.playerIdleState);
        }
        player.fx.CreateAfterImageFx();
    }

    public override void Exit()
    {
        base.Exit();
        player.skillManager.dashSkill.CloneOnArrival();
        player.SetVelocity(0, rb.linearVelocity.y);
        player.stat.MakeInvulnerable(false);
    }
}
