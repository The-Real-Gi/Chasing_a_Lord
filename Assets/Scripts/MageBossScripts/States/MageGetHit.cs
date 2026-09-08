using UnityEngine;

public class MageGetHit : MageState
{
    public MageGetHit(BossMageScript _bossMageScript, MageStateMachine _stateMachine, string _animBoolName)
        : base(_bossMageScript, _stateMachine, _animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        mageScript.UpdateGetHitState();
    }
}
