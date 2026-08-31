using Unity.VisualScripting;
using UnityEngine;

public class CrouchIdle : GroundedState
{
    public CrouchIdle(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }
     public override void Enter()
    {
        base.Enter();
        player.playerCollider.offset=player.crouchColliderOffset;
            player.playerCollider.size=player.collidersizeCrouch;
    
        player.rb.linearVelocity= new Vector2(0,player.rb.linearVelocityY);
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
    }
    public override void Update()
    {
        base.Update();
       
    }
}
