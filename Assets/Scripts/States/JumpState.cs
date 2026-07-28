using UnityEngine;

public class JumpState : PlayerState
{
    public JumpState(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.rb.linearVelocity= new Vector2(player.rb.linearVelocityX,player.jumpStrength);
        
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.rb.linearVelocityX=player.inputVector.x*player.moveSpeed*0.8f*Time.deltaTime;
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
