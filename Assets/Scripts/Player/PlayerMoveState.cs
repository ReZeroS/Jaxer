public class PlayerMoveState : PlayerGroundState
{

    
    public PlayerMoveState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }


    public override void Enter()
    {
        base.Enter(); 
        AudioManager.instance.PlaySFX(14);
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(calcSpeed(xInput), rb.velocity.y);
        if (xInput == 0 || player.IsWallDetected()) {
            stateMachine.ChangeState(player.playerIdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.StopSFX(14);
    }


    private float calcSpeed(float xInput)
    {
        return xInput * player.moveSpeed;
        // 计算目标速度和加速/减速率
        // float targetSpeed = xInput * maxSpeed;
        // float accelerationRate =
        //     physicsCheck.IsGrounded() ? accelerationTime : accelerationTime * airAccelerationMultiplier;
        // float decelerationRate =
        //     physicsCheck.IsGrounded() ? decelerationTime : decelerationTime * airDecelerationMultiplier;
        //
        // // 处理加速和减速
        // if (Mathf.Abs(targetSpeed) > 0.01f)
        // {
        //     currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, maxSpeed / accelerationRate * Time.deltaTime);
        // }
        // else
        // {
        //     currentSpeed = Mathf.MoveTowards(currentSpeed, 0, maxSpeed / decelerationRate * Time.deltaTime);
        // }
        //
        // return currentSpeed;
    }
    
    
}
