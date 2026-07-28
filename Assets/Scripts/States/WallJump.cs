using UnityEngine;

public class WallJump : PlayerState
{
   
    public WallJump(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.isWallJumping=true;
        player.Flip(player.facDir);
        
       player.rb.linearVelocity= new Vector2(player.wallJumpStrength*player.facDir*Time.deltaTime,player.jumpStrength);
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
        
        if(player.rb.linearVelocityY<=0)
        {
            player.isWallJumping=false;
            stateMachine.ChangeState(player.airState);
        }
       
    }
}
