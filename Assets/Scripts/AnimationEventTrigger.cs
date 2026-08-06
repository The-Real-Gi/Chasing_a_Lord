using UnityEngine;

public class AnimationEventTrigger : MonoBehaviour
{
   public void OnLLedgeClimbEnded()
    {
        PlayerScript player= GetComponentInParent<PlayerScript>();
        player.AnimationFinishCalled();
    }
}
