using UnityEngine;

public class GroundedState : PlayerState
{
    public GroundedState(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }

     public override void Enter()
    {
        base.Enter();
      
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public override void Update()
    {
        base.Update();
        if(player.inputVector.x!=0)
        {
            stateMachine.ChangeState(player.move);
        }else if(player.inputVector.x==0)
        {
            stateMachine.ChangeState(player.idle);
        }

        if(!player.isGrounded)
        {
            stateMachine.ChangeState(player.airState);
        }
    }
}
