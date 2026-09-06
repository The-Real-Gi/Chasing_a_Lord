using UnityEngine;

public class ArcherIdle : ArcherState
{
    private float idleTimer;

    public ArcherIdle(ArcherScript _archerScript, ArcherStateMachine _stateMachine, string _animBoolName) : base(_archerScript, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        idleTimer = archer.IdleDuration;
    }

    public override void Update()
    {
        base.Update();

        idleTimer -= Time.deltaTime;
        if (idleTimer <= 0f)
        {
            archer.Flip();
            if (archer.rb != null)
            {
                archer.rb.linearVelocity = new Vector2(0f, archer.rb.linearVelocity.y);
            }

            idleTimer = archer.IdleDuration;
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
