using UnityEngine;

public class ArcherAnimationTrigger : MonoBehaviour
{
   ArcherScript archer;
    void Awake()
    {
        archer= GetComponentInParent<ArcherScript>();
    }
    public void SignalToShoot()
    {
        archer.ShootSingal();
    }
}
