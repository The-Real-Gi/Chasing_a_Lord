using UnityEngine;

public class EnemyStateMachine 
    {
        public EnemyState currentState;
    

    public void Initialize(EnemyState startState)
    {
        currentState=startState;
        currentState.Enter();
    }

    public void ChangeState(EnemyState newState)
    {
        currentState.Exit();
        currentState= newState;
        currentState.Enter();
    }
}
