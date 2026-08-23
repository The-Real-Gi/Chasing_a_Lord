using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class AirState : PlayerState
{
    public AirState(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
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
        if(player.isTouchingLedge&&player.isWallDetected&& player.inputVector.x==player.facDir)
        {
            stateMachine.ChangeState(player.wallSlide);
        }
        if(player.isGrounded)
        {
            stateMachine.ChangeState(player.idle);
        }
       
    }
}
