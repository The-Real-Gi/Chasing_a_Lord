using UnityEngine;

public class MeleeAtt1 : PlayerState
{
    public MeleeAtt1(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }

     public override void Enter()
    {
        base.Enter();
       
       player.isAttacking=true;
    }

    public override void Exit()
    {
        base.Exit();
        player.finishAttack=false;
        player.isAttacking=false;
        
        
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public override void Update()
    {
        base.Update();
        if(player.finishAttack)
        {
            stateMachine.ChangeState(player.idle);
        }
       //wait fro trigger and then come back to idle state
       //create a window for following attack
    }
}
