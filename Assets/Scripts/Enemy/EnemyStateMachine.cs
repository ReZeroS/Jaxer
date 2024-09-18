public class EnemyStateMachine
{

    public EnemyState currentState { get; private set; }



    public void Initialize(EnemyState curState)
    {
        currentState = curState;
        currentState.Enter();
    }


    public void ChangeState(EnemyState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
    
}
