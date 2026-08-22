using UnityEngine;

public class SlideState : PlayerState
{
    public SlideState(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
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
        player.rb.linearVelocity = new Vector2(Mathf.Lerp(player.rb.linearVelocityX, 0f, 0.1f), player.rb.linearVelocityY);
    }
    public override void Update()
    {
        base.Update();
       if(Mathf.Abs(player.rb.linearVelocityX) <= 1f || !player.isGrounded)
        {
            stateMachine.ChangeState(player.idle);
        }
    }

}
