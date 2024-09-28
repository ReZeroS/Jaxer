using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{

    private Transform sword;
    
    public PlayerCatchSwordState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        sword = player.sword.transform;

        DustFxWhenCatchSword();

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePosition.x < player.transform.position.x && player.facingRight)
        {
            player.Flip();
        } else if (mousePosition.x > player.transform.position.x && !player.facingRight)
        {
            player.Flip();
        }

        rb.velocity = new Vector2(player.swordReturnImpact * -player.facingDir, rb.velocity.y);

    }

    private void DustFxWhenCatchSword()
    {
        player.fx.PlayDustFx();
        player.fx.ScreenShakeForSwordImpact(player.facingDir);
    }

    public override void Update()
    {
        base.Update();
        if (animationFinishedTriggerCalled)
        {
            stateMachine.ChangeState(player.playerIdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.SetBusyFor(0.1f);
    }
}
