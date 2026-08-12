using Unity.Android.Gradle.Manifest;
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
        Debug.Log("Starting ledgeclimb");
        previousGravityScale = player.rb.gravityScale;
        player.rb.gravityScale = 0f;
        player.rb.linearVelocity = Vector2.zero;
        currFacDir=player.facDir;
        
    }

    public override void Exit()
    {
        base.Exit();
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
            player.rb.linearVelocity= new Vector2(0,player.climbingUpSpeed*Time.deltaTime);
        }

        if(player.moveForward)
        {
             player.rb.linearVelocity= new Vector2(player.movingForwardSpeed*player.facDir*Time.deltaTime,0);
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
