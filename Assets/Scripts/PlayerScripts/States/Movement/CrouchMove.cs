using UnityEngine;

public class CrouchMove : GroundedState
{
    public CrouchMove(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    player.ApplyCrouchCollider();
      player.moveSpeed=player.baseRunSpeed;
    }

    public override void Exit()
    {
        base.Exit();
        player.ApplyBaseCollider();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        player.rb.linearVelocityX= player.moveSpeed*player.inputVector.x*Time.deltaTime/2;
       
    }
    public override void Update()
    {
        base.Update();
       

    }
}
