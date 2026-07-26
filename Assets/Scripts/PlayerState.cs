using UnityEngine;

public class PlayerState 
{
    protected PlayerScript player;
    protected string animName;
    protected StateMachine stateMachine;

    public PlayerState(PlayerScript _player, string _animName, StateMachine _statemachine)
    {
        this.player=_player;
        this.animName=_animName;
        this.stateMachine=_statemachine;
      
    }

    
    public virtual void Enter()
    {
        player.anim.SetBool(animName,true);
    }

    public virtual void Update()
    {
        
    }
    public virtual void FixedUpdate()
    {
        
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animName,false);
    }
    
}
