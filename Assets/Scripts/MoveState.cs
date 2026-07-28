using Unity.VisualScripting;
using UnityEngine;

public class MoveState : PlayerState
{
    public MoveState(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
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
        player.rb.linearVelocityX= player.moveSpeed*player.inputVector.x*Time.deltaTime;
    }
    public override void Update()
    {
        base.Update();
        if(player.inputVector.x==0)
        {
            stateMachine.ChangeState(player.idle);
        }
    }
}
