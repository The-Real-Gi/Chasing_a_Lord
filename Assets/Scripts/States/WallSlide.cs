using UnityEngine;

public class WallSlide : PlayerState
{
    public WallSlide(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
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
        player.rb.linearVelocityY= player.rb.linearVelocityY*0.5f;
    }
    public override void Update()
    {
        base.Update();
        if(player.inputVector.x!=player.facDir)
        {
            stateMachine.ChangeState(player.airState);
        }
        
       
    }
}
