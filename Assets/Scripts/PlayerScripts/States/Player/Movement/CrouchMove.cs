using UnityEngine;

public class CrouchMove : GroundedState
{
    public CrouchMove(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    player.playerCollider.offset=player.crouchColliderOffset;
    player.playerCollider.size=player.collidersizeCrouch;
      player.moveSpeed=player.baseRunSpeed;
    }

    public override void Exit()
    {
        base.Exit();
        player.playerCollider.offset=player.baseCollider;
        player.playerCollider.size= player.sizeCollider;
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
