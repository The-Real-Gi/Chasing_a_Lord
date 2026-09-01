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
    
    public void Attack2Finish()
    {
        enemy.Attack2Finish();
    }
    public void DealDamage()
    {
        enemy.TryDealDamage();
    }
    public void GetHitFinish()
    {
        
    }
}
