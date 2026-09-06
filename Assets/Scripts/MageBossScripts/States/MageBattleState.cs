using UnityEngine;

public class MageBattleState : MageState
{
    public MageBattleState(BossMageScript _bossMageScript, MageStateMachine _stateMachine, string _animBoolName) : base(_bossMageScript, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (mageScript.isWallDetected || !mageScript.isPlayerSeen)
        {
            stateMachine.ChangeState(mageScript.mageIdle);
            return;
        }

        int randomAttack = Random.Range(0, 2);
        if (randomAttack == 0)
        {
            stateMachine.ChangeState(mageScript.mageAttack1);
        }
        else
        {
            stateMachine.ChangeState(mageScript.mageAttack2);
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
