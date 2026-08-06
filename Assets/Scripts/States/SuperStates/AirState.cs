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
        if(!player.isWallJumping&&!player.isHandging)
        {
        player.rb.linearVelocityX=player.inputVector.x*player.moveSpeed*0.8f*Time.deltaTime;
        }

    }
    public override void Update()
    {
        base.Update();
        if(player.isWallDetected&& player.inputVector.x==player.facDir&& !player.isHandging)
        {
            stateMachine.ChangeState(player.wallSlide);
        }
        if(player.isGrounded)
        {
            stateMachine.ChangeState(player.idle);
        }
       
    }
}
