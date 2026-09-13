using UnityEngine;

public class LedgeClimbState : PlayerState
{
     private float previousGravityScale;
      int currFacDir;
    public LedgeClimbState(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        player.isClimbingLedge=true;

        previousGravityScale = player.rb.gravityScale;
        player.rb.gravityScale = 0f;
        player.rb.linearVelocity = Vector2.zero;
        currFacDir=player.facDir;
        player.ApplyCrouchCollider();
        
    }

    public override void Exit()
    {
        base.Exit();
        player.ApplyBaseCollider();
        player.isClimbingLedge=false;
        player.canClimbLedge=false;
        player.finishedCLimb=false;
        player.moveUp=false;
        player.moveForward=false;
        player.rb.gravityScale = previousGravityScale;
        player.rb.linearVelocity = Vector2.zero;
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if(player.moveUp)
        {
            player.rb.linearVelocity = new Vector2(0f, player.climbingUpSpeed * Time.deltaTime);
        }

        if(player.moveForward)
        {
             player.rb.linearVelocity= new Vector2(player.movingForwardSpeed*player.facDir*Time.deltaTime,0);
        }
        else if (!player.moveUp)
        {
            // Do not retain horizontal momentum between climb animation
            // phases. Forward motion is only applied explicitly above.
            player.rb.linearVelocityX = 0f;
        }
        
    }
    public override void Update()
    {
        base.Update();
        if(player.finishedCLimb)
        {
            stateMachine.ChangeState(player.idle);
        }
        
       
    }
}
