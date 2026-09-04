using UnityEngine;

public class MageState 
{
    protected BossMageScript mageScript;
    protected MageStateMachine stateMachine;
    protected string animBoolName;

    public MageState(BossMageScript _bossMageScript, MageStateMachine _stateMachine, string _animBoolName)
    {
        this.mageScript = _bossMageScript;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }
    public virtual void Enter()
    {
        mageScript.anim.SetBool(animBoolName,true);
    }

    public virtual void Update()
    {
        
    }

    public virtual void FixedUpdate()
    {
        
    }

    public virtual void Exit()
    {
        mageScript.anim.SetBool(animBoolName,false);
    }
}
