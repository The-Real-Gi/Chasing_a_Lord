using UnityEngine;

public class SlideState : PlayerState
{
    public SlideState(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
         player.playerCollider.offset=player.crouchColliderOffset;
        player.playerCollider.size=player.collidersizeCrouch;
        
    }

    public override void Exit()
    {
        base.Exit();
        player.playerCollider.offset=player.baseCollider;
        player.playerCollider.size= player.sizeCollider;
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.rb.linearVelocity = new Vector2(Mathf.Lerp(player.rb.linearVelocityX, 0f, 0.03f), player.rb.linearVelocityY);
    }
    public override void Update()
    {
        base.Update();
        if (!player.isGrounded)
        {
            stateMachine.ChangeState(player.idle);
            return;
        }

        if (Mathf.Abs(player.rb.linearVelocityX) <= 1f)
        {
            stateMachine.ChangeState(player.isForcedCrouch ? player.crouchIdle : player.idle);
        }
    }

}
