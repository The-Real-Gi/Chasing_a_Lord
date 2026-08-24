using UnityEngine;

public class IdleState : GroundedState
{
    public IdleState(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.rb.linearVelocity= new Vector2(0,player.rb.linearVelocityY);
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
       
    }
}
