using UnityEngine;

public class ShieldBlock : PlayerState
{
    public ShieldBlock(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }

    public override void Enter()
    {    
        base.Enter();
        player.rb.linearVelocity= new Vector2(0,player.rb.linearVelocityY);
    }

    public override void Update()
    {
        base.Update();
        player.input.Movement.Block.canceled+= ctx=>stateMachine.ChangeState(player.idle);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
