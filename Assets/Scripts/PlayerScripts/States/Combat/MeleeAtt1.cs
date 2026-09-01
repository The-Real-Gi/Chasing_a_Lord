using UnityEngine;

public class MeleeAtt1 : PlayerState
{
    private float currentXVelocity;

    public MeleeAtt1(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.isAttacking=true;
        player.meleeAtt1DamageDealt = false;
        currentXVelocity = player.rb.linearVelocity.x;
    }

    public override void Exit()
    {
        base.Exit();
        player.finishAttack=false;
        player.isAttacking=false;
        player.meleeAtt1DamageDealt = false;
        player.rb.linearVelocity = Vector2.zero;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        float targetVelocity = player.meleeAtt1MoveSpeed * player.facDir;
        if (player.meleeAtt1DamageDealt)
        {
            targetVelocity = -player.meleeAtt1MoveSpeed * player.facDir;
        }

        float blendSpeed = Mathf.Clamp01(player.meleeAttackBlendSpeed * Time.fixedDeltaTime);
        currentXVelocity = Mathf.Lerp(currentXVelocity, targetVelocity, blendSpeed);
        player.rb.linearVelocity = new Vector2(currentXVelocity, player.rb.linearVelocity.y);
    }

    public override void Update()
    {
        base.Update();
        if(player.finishAttack)
        {
            stateMachine.ChangeState(player.idle);
        }
    }
}
