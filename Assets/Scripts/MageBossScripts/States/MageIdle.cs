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
        mageScript.FacePlayer();
        idleTimer = mageScript.IdleDuration;
    }

    public override void Update()
    {
        base.Update();

        idleTimer -= Time.deltaTime;
        if (idleTimer <= 0f)
        {
            if (mageScript.isWallDetected)
            {
                idleTimer = mageScript.IdleDuration;
                return;
            }

            stateMachine.ChangeState(mageScript.mageMove);
            return;
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
