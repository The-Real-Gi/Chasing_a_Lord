using UnityEngine;

public class MeleeRun : PlayerState
{
    private float currentXVelocity;
    private float moveDirection;
    private float accelerationTimer;
    private float targetSpeed;

    public MeleeRun(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.isAttacking=true;

        float startingVelocity = player.rb.linearVelocity.x;
        moveDirection = Mathf.Abs(startingVelocity) > 0.01f ? Mathf.Sign(startingVelocity) : player.facDir;
        currentXVelocity = startingVelocity;
        accelerationTimer = 0f;

        if (Mathf.Abs(startingVelocity) > 0.01f)
        {
            targetSpeed = Mathf.Abs(startingVelocity);
        }
        else
        {
            targetSpeed = player.baseRunSpeed;
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.finishAttack=false;
        player.isAttacking=false;
        player.rb.linearVelocity = new Vector2(0f, player.rb.linearVelocity.y);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (Mathf.Abs(player.rb.linearVelocity.x) < 0.01f)
        {
            accelerationTimer += Time.fixedDeltaTime;

            if (accelerationTimer < player.meleeRunAccelerationTimer)
            {
                float targetX = moveDirection * targetSpeed;
                currentXVelocity = Mathf.Lerp(currentXVelocity, targetX, player.meleeRunAccelerationSpeed);
                player.rb.linearVelocity = new Vector2(currentXVelocity, player.rb.linearVelocity.y);
                return;
            }
        }

        currentXVelocity = Mathf.Lerp(currentXVelocity, 0f, 0.04f);
        player.rb.linearVelocity = new Vector2(currentXVelocity, player.rb.linearVelocity.y);

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
