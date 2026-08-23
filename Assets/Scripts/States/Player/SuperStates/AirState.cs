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
        
        if(Mathf.Abs(player.inputVector.x)>0.01f)
        {
            player.rb.linearVelocityX=player.moveSpeed*player.inputVector.x*Time.deltaTime;
        }
        

    }
    public override void Update()
    {
        base.Update();
        if(player.isWallDetected && player.inputVector.x * player.facDir > 0.1f)
        {
            stateMachine.ChangeState(player.wallSlide);
            return;
        }

        if(player.isGrounded)
        {
            stateMachine.ChangeState(player.idle);
        }
       
    }
}
