using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimSwordState : PlayerState
{
   

    public PlayerAnimSwordState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        player.skillManager.swordSkill.DotsActive(true);
        player.StartCoroutine("BusyFor", 0.2f);
    }

    public override void Update()
    {
        base.Update();
        
        player.SetZeroVelocity();
        
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            stateMachine.ChangeState(player.playerIdleState);
        }
        
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePosition.x < player.transform.position.x && player.facingRight)
        {
            player.Flip();
        } else if (mousePosition.x > player.transform.position.x && !player.facingRight)
        {
            player.Flip();
        }
        
    }

    public override void Exit()
    {
        base.Exit();
    }
}
