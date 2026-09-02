using UnityEngine;

public class CrouchAttack : PlayerState
{
    public CrouchAttack(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.ApplyCrouchCollider();
        player.isAttacking = true;
    }

    public override void Update()
    {
        base.Update();
        if(player.finishAttack)
        {
            stateMachine.ChangeState(player.crouchIdle);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
        player.ApplyBaseCollider();
        player.isAttacking = false;
         player.crouchMeleeDamageDealt=false;
        player.finishAttack=false;
    }
}
