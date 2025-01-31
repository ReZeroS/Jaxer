namespace ReZeros.Jaxer.PlayerBase
{
    public class PlayerDeathState : PlayerState
    {

        public PlayerDeathState(PlayerStateMachine stateMachine, MainPlayer mainPlayer, string animBoolName) : base(stateMachine, mainPlayer, animBoolName)
        {
        
        
        }
    
    
        public override void Enter()
        {
            base.Enter();
            mainPlayer.SetZeroVelocity();
            // GameObject.Find("Canvas").GetComponent<UI>().fadeScreen.FadeOut();
            // WaitForSeconds wait = new WaitForSeconds(2f);
            // GameManager.instance.RestartGame();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
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
}
