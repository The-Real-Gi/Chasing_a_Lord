using UnityEngine;

public class AnimationEventTrigger : MonoBehaviour
{   PlayerScript player;
    void Awake()
    {
        player= GetComponentInParent<PlayerScript>();
    }
   public void OnLLedgeClimbEnded()
    {
       
        player.AnimationFinishCalled();
    }
    public void ClimningUp()
    {
        player.AniimationClimbCalled();
    }
    public void MoveForward()
    {
        player.AnimationMoveForwardCalled();
    }

    public void AttackEnded()
    {
        player.AttackFinish();
    }
    public void CheckToDealDamage()
    {
        player.Attack1Checks();

        if (player.stateMachine.currentState == player.meleeAtt1)
        {
            player.meleeAtt1DamageDealt = true;
        }
        else if (player.stateMachine.currentState == player.meleeAtt2)
        {
            player.meleeAtt2DamageDealt = true;
        }
        else if (player.stateMachine.currentState == player.meleeSpin)
        {
            player.meleeSpinDamageDealt = true;
            player.rb.linearVelocity = Vector2.zero;
        }
        else if (player.stateMachine.currentState == player.kick)
        {
            player.kickDamageDealt = true;
        }
    }

    // do func for all types of attack
    
}
