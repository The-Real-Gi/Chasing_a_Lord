using UnityEngine;

public class ArcherState 
{
   protected ArcherScript archer;
   protected ArcherStateMachine stateMachine;
   protected string animBoolName;
   public ArcherState(ArcherScript _archerScript, ArcherStateMachine _stateMachine, string _animBoolName)
    {
        archer=_archerScript;
        stateMachine = _stateMachine;
        animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        archer.anim.SetBool(animBoolName,true);
    }

    public virtual void Update()
    {
        
    }

    public virtual void FixedUpdate()
    {
        
    }

    public virtual void Exit()
    {
        archer.anim.SetBool(animBoolName,false);
    }
}
