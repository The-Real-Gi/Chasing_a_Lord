using UnityEngine;

public class JumpState : AirState
{
    public JumpState(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
      
        player.rb.AddForce(new Vector2(0,player.jumpStrength),ForceMode2D.Impulse);
        player.jumpCount=1;
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
            stateMachine.ChangeState(player.airState);
        }
        
    }
}
