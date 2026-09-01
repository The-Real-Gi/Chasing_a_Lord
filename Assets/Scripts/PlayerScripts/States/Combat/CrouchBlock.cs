using UnityEngine;

public class CrouchBlock : PlayerState
{
    public CrouchBlock(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
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
        player.input.Movement.CrouchBlock.canceled+= ctx => stateMachine.ChangeState(player.crouchIdle);
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
