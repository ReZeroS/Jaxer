using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackholeState : PlayerState
{

    private float flyTime = 0.4f;
    private bool skillUsed;


    private float defaultGravity;
    
    
    public PlayerBlackholeState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }


    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        defaultGravity = player.rb.gravityScale;
        
        skillUsed = false;
        stateTimer = flyTime;
        rb.gravityScale = 0;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 15);
        }

        if (stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -0.1f);
            if (!skillUsed)
            {
                if (player.skillManager.blackholeSkill.CanUseSkill())
                {
                    skillUsed = true;
                }
            }
        }

        if (player.skillManager.blackholeSkill.SkillCompleted())
        {
            stateMachine.ChangeState(player.playerAirState);
        }
        
        
    }

    public override void Exit()
    {
        base.Exit();
        player.rb.gravityScale = defaultGravity;
        player.fx.MakeTransparent(false);
    }

}
