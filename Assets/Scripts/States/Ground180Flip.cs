using UnityEngine;

public class Ground180Flip : PlayerState
{
    private int flipDirection;

    public Ground180Flip(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }
      public override void Enter()
    {
        base.Enter();
        player.flipEnded = false;
        player.isFlipping=true;
        player.flipCooldownTimer = player.flipCooldown;
        flipDirection = player.inputVector.x >= 0f ? 1 : -1;
        player.anim.speed=2;
       
    }

    public override void Exit()
    {
        base.Exit();
        player.Flip(flipDirection);
        player.anim.speed=1f;
        player.isFlipping=false;
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.rb.linearVelocityX=0;
    }
    public override void Update()
    {
        base.Update();
        if(player.flipEnded)
        {
            stateMachine.ChangeState(player.idle);
        }
       
    }
}
