using UnityEngine;

public class ArcherStateMachine 
{
    public ArcherState currentState;

    public void Initialize (ArcherState startState)
    {
        currentState= startState;
        currentState.Enter();
    
    }

    public void ChangeState(ArcherState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
