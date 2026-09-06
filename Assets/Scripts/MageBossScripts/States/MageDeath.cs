using UnityEngine;

public class MageDeath : MageState
{
    public MageDeath(BossMageScript _bossMageScript, MageStateMachine _stateMachine, string _animBoolName) : base(_bossMageScript, _stateMachine, _animBoolName)
    {
    }

     public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        mageScript.rb.linearVelocity= Vector2.zero;
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
