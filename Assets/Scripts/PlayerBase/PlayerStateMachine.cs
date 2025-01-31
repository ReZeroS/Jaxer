namespace ReZeros.Jaxer.PlayerBase
{
    public class PlayerStateMachine 
    {
        public PlayerState currentState { get; private set; }


        public void Initialize(PlayerState playerState)
        {
            currentState = playerState;
            currentState.Enter();
        }

        public void ChangeState(PlayerState newState)
        {
            currentState.Exit(); 
            currentState = newState;
            currentState.Enter();
        }
    
    
    
    
    }
}
