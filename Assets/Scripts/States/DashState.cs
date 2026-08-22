using UnityEngine;

public class DashState : PlayerState
{
    public DashState(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
       player.timer=player.dashingTime;
    }

    public override void Exit()
    {
        base.Exit();
        player.cooldownTimer=player.dashCooldown;
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.rb.linearVelocity= new Vector2(player.facDir*player.dashSpeed*Time.deltaTime,0);
    }
    public override void Update()
    {
        base.Update();
       player.timer-=Time.deltaTime;
       if(player.timer<=0)
        {
            stateMachine.ChangeState(player.idle);
        }
    }
}
