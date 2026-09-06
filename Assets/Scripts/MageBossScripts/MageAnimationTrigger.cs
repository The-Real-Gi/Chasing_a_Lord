using UnityEngine;

public class MageAnimationTrigger : MonoBehaviour
{
   BossMageScript boss;
    void Awake()
    {
        boss= GetComponentInParent<BossMageScript>();
    }

    public void AttackFinish()
    {
        boss.AttackFinisher();
    }

    public void SpawnAttack()
    {
        boss.SpawnAttackObj();
    }
}
