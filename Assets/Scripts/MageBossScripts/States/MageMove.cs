using UnityEngine;

public class MageMove : MageState
{
    public MageMove(BossMageScript _bossMageScript, MageStateMachine _stateMachine, string _animBoolName) : base(_bossMageScript, _stateMachine, _animBoolName)
    {
    }

     public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (mageScript.isWallDetected)
        {
            stateMachine.ChangeState(mageScript.mageIdle);
            return;
        }

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (mageScript.rb != null)
        {
            mageScript.rb.linearVelocity = new Vector2(mageScript.facDir * mageScript.MoveSpeed, mageScript.rb.linearVelocity.y);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
