using UnityEngine;

public class Kick : PlayerState
{
    private float currentXVelocity;

    public Kick(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.isAttacking=true;
        currentXVelocity = player.rb.linearVelocity.x;
    }

    public override void Exit()
    {
        base.Exit();
        player.finishAttack=false;
        player.isAttacking=false;
        player.ResetKickMovement();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        float targetVelocity = player.kickMoveSpeed * player.facDir;
        if (player.kickDamageDealt)
        {
            targetVelocity = -player.kickMoveSpeed * player.facDir;
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
