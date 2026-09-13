using UnityEngine;

public class WallSlide : PlayerState
{
    public WallSlide(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.jumpCount=0;
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        // Do not carry horizontal momentum into the slide.
        player.rb.linearVelocityX = 0f;
        player.rb.linearVelocityY= player.rb.linearVelocityY*0.5f;
    }
    public override void Update()
    {
        base.Update();
        // Use a relative dead-zone instead of exact equality. Also leave the
        // state when contact is lost or the player reaches the ground.
        if(!player.isWallDetected || player.isGrounded
            || player.inputVector.x * player.facDir <= 0.1f)
        {
            stateMachine.ChangeState(player.airState);
        }
        
       
    }
}
