using UnityEngine;

public class MageAttack2 : MageState
{
    public MageAttack2(BossMageScript _bossMageScript, MageStateMachine _stateMachine, string _animBoolName) : base(_bossMageScript, _stateMachine, _animBoolName)
    {
    }

     public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if(mageScript.attackEnded)
        {
            stateMachine.ChangeState(mageScript.mageBattleState);
        }

        if(mageScript.spawnObj)
        {
            int attnum=Random.Range(1,3);

            if(attnum==1)
            {
                
            }else if (attnum==2)
            {
                
            }else if (attnum==3)
            {
                
            }
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
        mageScript.attackEnded=false;
        mageScript.spawnObj = false;
    }
}
