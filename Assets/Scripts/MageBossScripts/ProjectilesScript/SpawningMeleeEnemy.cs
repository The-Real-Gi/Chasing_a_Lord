using UnityEngine;

public class SpawningMeleeEnemy : MonoBehaviour
{

    private Animator anim;
    public GameObject meleeEnemy;
    private int facingDirection = 1;

    void Awake()
    {
        anim=GetComponent<Animator>();
        anim.SetBool("SmokeFinish",true);
    }

    public void SetFacingDirection(int direction)
    {
        facingDirection = direction >= 0 ? 1 : -1;
    }

    public void SpawnEnemy()
    {
        if (meleeEnemy == null)
        {
            return;
        }

        GameObject spawnedEnemy = Instantiate(meleeEnemy, transform.position, Quaternion.identity);
        EnemyScript enemyScript = spawnedEnemy.GetComponent<EnemyScript>();
        if (enemyScript != null)
        {
            enemyScript.SetFacingDirection(facingDirection);
        }

    }

    public void DestroyCloud()
    {
        Destroy(gameObject);
    }
}
