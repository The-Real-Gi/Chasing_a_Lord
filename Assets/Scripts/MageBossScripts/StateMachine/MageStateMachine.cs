using UnityEngine;

public class MageStateMachine 
{
public  MageState currentState;

public void Initialize(MageState startState)
    {
        currentState = startState;
        currentState.Enter();
    }

    public void ChangeState(MageState newState)
    {
        currentState.Exit();
        currentState= newState;
        currentState.Enter();
    }

    
}
