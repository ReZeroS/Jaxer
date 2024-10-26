using System;

public class StateTransitionCheck
{
    private readonly Func<bool> condition;
    private readonly Func<PlayerState> getNextState;

    public StateTransitionCheck(Func<bool> condition, Func<PlayerState> getNextState)
    {
        this.condition = condition;
        this.getNextState = getNextState;
    }

    public bool TryTransition(PlayerStateMachine stateMachine)
    {
        if (!condition()) return false;
        
        stateMachine.ChangeState(getNextState());
        return true;
    }
}