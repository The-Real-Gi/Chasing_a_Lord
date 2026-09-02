using UnityEngine;
using UnityEngine.InputSystem;

public class CrouchBlock : PlayerState
{
    public CrouchBlock(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }

   public override void Enter()
    {
        base.Enter();
        player.ApplyCrouchCollider();
        player.input.Movement.CrouchBlock.canceled += OnCrouchBlockCanceled;
        player.rb.linearVelocity= new Vector2(0,player.rb.linearVelocityY);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
        player.input.Movement.CrouchBlock.canceled -= OnCrouchBlockCanceled;
        player.ApplyBaseCollider();
    }

    private void OnCrouchBlockCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(player.crouchIdle);
    }
}
