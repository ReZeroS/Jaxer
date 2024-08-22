using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{

    public int comboCounter { get; private set; }
    private float lastTimeAttacked;
    private int comboWindow = 2;
    public PlayerPrimaryAttackState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        xInput = 0; // fix the attack direction
        if (comboCounter > 2 || lastTimeAttacked + comboWindow < Time.time)
        {
            comboCounter = 0;
        }
        player.animator.SetInteger("ComboCounter", comboCounter);

        #region AttackDir
        float attackDir = player.facingDir;
        if (xInput != 0)
        {
            attackDir = xInput;
        }
        #endregion
       
        
        player.SetVelocity(player.attackMovements[comboCounter].x * attackDir, player.attackMovements[comboCounter].y);
        
        stateTimer = 0.1f;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            rb.velocity = new Vector2(0, 0);
        }
        
        if (animationFinishedTriggerCalled)
        {
            stateMachine.ChangeState(player.playerIdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        comboCounter++;
        lastTimeAttacked = Time.time;
        // for idlestate
        player.StartCoroutine("BusyFor", 0.1f);
    }
}
