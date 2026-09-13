using UnityEngine;

public class WallHangState : PlayerState
{
    private float previousGravityScale;
    int currFacDir;

    public WallHangState(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }


    public override void Enter()
    {
        base.Enter();
        player.isHandging=true;
        previousGravityScale = player.rb.gravityScale;
        player.rb.gravityScale = 0f;
        player.rb.linearVelocity = Vector2.zero;
        currFacDir=player.facDir;
        player.canClimbLedge=true;
        // Save the wall contact point before calculating the climb targets.
        // Without this, ledgePos1/2 are derived from the default (0, 0).
        player.ledgePosBot = player.wallCheck.position;
        
    }

    public override void Exit()
    {
        base.Exit();
        player.isHandging=false;
        
        player.rb.gravityScale = previousGravityScale;
        player.rb.linearVelocity = Vector2.zero;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        // A hang is stationary; clear any horizontal momentum inherited from
        // the airborne state.
        player.rb.linearVelocity = Vector2.zero;
    }
    public override void Update()
    {
        base.Update();
         
          if(player.inputVector.y>0)
        {   
            stateMachine.ChangeState(player.ledgeClimbState);
            return;
        }
        if(!player.isWallDetected || player.inputVector.y < 0f)
        {
            player.ledgeDetected=false;
            stateMachine.ChangeState(player.airState);
            return;
        }
            if(player.isFacingRight)
            {
                player.ledgePos1= new Vector2(Mathf.Floor(player.ledgePosBot.x+player.wallCheckDistance)-player.ledgeClimbXOffset1,Mathf.Floor(player.ledgePosBot.y)+player.ledgeClimbYOffset1);
                player.ledgePos2= new Vector2(Mathf.Floor(player.ledgePosBot.x+player.wallCheckDistance)+player.ledgeClimbXoffset2,Mathf.Floor(player.ledgePosBot.y)+player.ledgeclimbYOffset2);
            }
            else
            {
                player.ledgePos1= new Vector2(Mathf.Ceil(player.ledgePosBot.x-player.wallCheckDistance)+player.ledgeClimbXOffset1,Mathf.Floor(player.ledgePosBot.y)+player.ledgeClimbYOffset1);
                player.ledgePos2= new Vector2(Mathf.Ceil(player.ledgePosBot.x-player.wallCheckDistance)- player.ledgeClimbXOffset1,Mathf.Floor(player.ledgePosBot.y)+player.ledgeClimbYOffset1);
            }
       
    }
}
