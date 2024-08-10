using System.Collections;
using System.Collections.Generic;
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

    public override void Update()
    {
        base.Update();
        if (animationFinishedTriggerCalled)
        {
            stateMachine.ChangeState(player.playerIdleState);
        }
        {
            
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", 0.1f);
    }
}
