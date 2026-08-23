using UnityEngine;

public class GroundedState : PlayerState
{
    public GroundedState(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }

     public override void Enter()
    {
        base.Enter();
      player.isWallJumping=false;
      player.jumpCount=0;
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
        if(!player.isGrounded)
        {
            stateMachine.ChangeState(player.airState);
            return;
        }

        if(player.inputVector.x!=0)
        {
            if(player.inputVector.y<0)
            {   if(Mathf.Abs(player.rb.linearVelocityX)<=2)
                {
                    
                stateMachine.ChangeState(player.crouchMove);
                }
                if(Mathf.Abs(player.rb.linearVelocityX)>2)
                {
                    stateMachine.ChangeState(player.slideState);
                }
            }
            else
            {
              if(stateMachine.currentState!=player.move)
                {
                stateMachine.ChangeState(player.move);
                    
                }  
            }

        }else if(player.inputVector.x==0)
        {
            if(player.inputVector.y<0)
            {
                stateMachine.ChangeState(player.crouchIdle);
            }
            else
            {
            stateMachine.ChangeState(player.idle);
                
            }
        }

    }
}
