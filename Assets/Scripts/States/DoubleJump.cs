using UnityEngine;

public class DoubleJump : AirState
{
    public DoubleJump(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }

        public override void Enter()
    {
        base.Enter();
        player.rb.linearVelocityY=0;
       player.rb.AddForce(new Vector2(0,player.jumpStrength),ForceMode2D.Impulse);
        player.jumpCount++;
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
        if(player.rb.linearVelocityY<0)
        {
            stateMachine.ChangeState(player.airState);
        }
       
    }
}
