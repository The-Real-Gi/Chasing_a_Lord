using UnityEngine;

public class GetHit : PlayerState
{
    public GetHit(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
       
        player.getHitTimer = player.getHitDuration;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        // Calculate how much time has elapsed (0 to 1)
        float elapsedRatio = 1f - (player.getHitTimer / player.getHitDuration);
        // Lerp from full knockback force to 0 based on elapsed time
        float currentForce = Mathf.Lerp(player.getHitKnockbackForce, 0f, elapsedRatio);
        player.rb.linearVelocity = new Vector2(currentForce * player.getHitKnockbackDirection, player.rb.linearVelocity.y);
    }

    public override void Update()
    {
        base.Update();
        player.getHitTimer -= Time.deltaTime;

        if (player.getHitTimer <= 0f)
        {
            stateMachine.ChangeState(player.idle);
        }
    }
}
