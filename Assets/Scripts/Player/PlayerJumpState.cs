using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{

    public PlayerJumpState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        // 空洞骑士信仰之跃时走这个，不能转向
        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
        // 这里后续看看
        // player.setVelocity(rb.velocity.x, player.jumpForce);
    }

    public override void Update()
    {
        base.Update();

        if (rb.velocity.y < 0)
        {
            stateMachine.ChangeState(player.playerAirState);
        }
        
    }

    public override void Exit()
    {
        base.Exit();
    }
}
