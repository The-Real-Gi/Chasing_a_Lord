using Unity.VisualScripting;
using UnityEngine;

public class MoveState : GroundedState
{
    public MoveState(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.moveSpeed=player.baseRunSpeed;
        Debug.Log("entered the base move state");
        
    }

    public override void Exit()
    {
        base.Exit();
     
        player.anim.speed= 1;
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.moveSpeed=player.moveSpeed*1.1f;
        player.anim.speed= player.anim.speed*1.1f;
        if(player.anim.speed>=3)
        {
            player.anim.speed=3;
        }
        if(player.moveSpeed>=player.maxRunSpeed)
        {
            player.moveSpeed=player.maxRunSpeed;
        }
        player.rb.linearVelocityX= player.moveSpeed*player.inputVector.x*Time.deltaTime;
    }
    public override void Update()
    {
        
        base.Update();
       
    }
}
