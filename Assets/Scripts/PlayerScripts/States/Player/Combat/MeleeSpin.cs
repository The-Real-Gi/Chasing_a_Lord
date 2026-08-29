using UnityEngine;

public class MeleeSpin : PlayerState
{
    private float currentXVelocity;

    public MeleeSpin(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.isAttacking=true;
        player.meleeSpinDamageDealt = false;
        currentXVelocity = player.rb.linearVelocity.x;

        if (Mathf.Abs(currentXVelocity) < 0.01f)
        {
            Vector2 push = new Vector2(player.meleeSpinPushForce * player.facDir, player.meleeSpinUpForce);
            player.rb.AddForce(push, ForceMode2D.Impulse);
            currentXVelocity = player.rb.linearVelocity.x;
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.finishAttack=false;
        player.isAttacking=false;
        player.meleeSpinDamageDealt = false;
        player.rb.linearVelocity = Vector2.zero;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (player.meleeSpinDamageDealt)
        {
            currentXVelocity = 0f;
            player.rb.linearVelocity = Vector2.zero;
            return;
        }

        float slowdownStrength = Mathf.Clamp01(player.meleeSpinSlowdownRate);
        currentXVelocity = Mathf.Lerp(currentXVelocity, 0f, slowdownStrength * Time.fixedDeltaTime);

        float liftVelocity = Mathf.Lerp(player.rb.linearVelocity.y, player.meleeSpinUpForce, 0.2f * Time.fixedDeltaTime);
        player.rb.linearVelocity = new Vector2(currentXVelocity, liftVelocity);

        if (Mathf.Abs(currentXVelocity) < 0.05f)
        {
            currentXVelocity = 0f;
            player.rb.linearVelocity = new Vector2(0f, player.rb.linearVelocity.y);
        }
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
