using UnityEngine;

public class MageIdle : MageState
{
    private float idleTimer;

    public MageIdle(BossMageScript _bossMageScript, MageStateMachine _stateMachine, string _animBoolName) : base(_bossMageScript, _stateMachine, _animBoolName)
    {
    }

     public override void Enter()
    {
        base.Enter();
        idleTimer = mageScript.IdleDuration;
    }

    public override void Update()
    {
        base.Update();

        idleTimer -= Time.deltaTime;
        if (idleTimer <= 0f)
        {
            mageScript.Flip();
            if (mageScript.rb != null)
            {
                mageScript.rb.linearVelocity = new Vector2(0f, mageScript.rb.linearVelocity.y);
            }

            idleTimer = mageScript.IdleDuration;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
