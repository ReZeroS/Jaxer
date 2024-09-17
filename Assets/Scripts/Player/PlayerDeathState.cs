public class PlayerDeathState : PlayerState
{

    public PlayerDeathState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
        
        
    }
    
    
    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();
        // GameObject.Find("Canvas").GetComponent<UI>().fadeScreen.FadeOut();
        // WaitForSeconds wait = new WaitForSeconds(2f);
        // GameManager.instance.RestartGame();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

}
