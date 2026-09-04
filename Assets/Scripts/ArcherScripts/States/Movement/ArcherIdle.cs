using UnityEngine;

public class ArcherIdle : ArcherState
{
    public ArcherIdle(ArcherScript _archerScript, ArcherStateMachine _stateMachine, string _animBoolName) : base(_archerScript, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
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
