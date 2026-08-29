using UnityEngine;

public class MeleeAtt2 : PlayerState
{
    private float currentXVelocity;

    public MeleeAtt2(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.isAttacking=true;
        player.meleeAtt2DamageDealt = false;
        currentXVelocity = player.rb.linearVelocity.x;
    }

    public override void Exit()
    {
        base.Exit();
        player.finishAttack=false;
        player.isAttacking=false;
        player.meleeAtt2DamageDealt = false;
        player.rb.linearVelocity = Vector2.zero;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        float targetVelocity = player.meleeAtt2MoveSpeed * player.facDir;
        if (player.meleeAtt2DamageDealt)
        {
            targetVelocity = -player.meleeAtt2MoveSpeed * player.facDir;
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
