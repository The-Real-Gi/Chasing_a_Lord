using UnityEngine;

public class GetHit : PlayerState
{
    public GetHit(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.rb.linearVelocity = Vector2.zero;
        player.getHitTimer = player.getHitDuration;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.rb.linearVelocity = new Vector2(0f, player.rb.linearVelocity.y);
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
