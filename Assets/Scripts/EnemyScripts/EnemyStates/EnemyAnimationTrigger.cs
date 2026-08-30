using UnityEngine;

public class EnemyAnimationTrigger : MonoBehaviour
{
    EnemyScript enemy;
    void Awake()
    {
        enemy = GetComponentInParent<EnemyScript>();
    }
    public void Attack1Finish()
    {
        enemy.AttackFinish();
    }

    public void DealDamage()
    {
        //signal to check for player to deal damage
    }
}
