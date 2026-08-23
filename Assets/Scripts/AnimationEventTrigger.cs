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
    
}
