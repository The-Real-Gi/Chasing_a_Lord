using UnityEngine;

public class SpawningMeleeEnemy : MonoBehaviour
{

    private Animator anim;
    public GameObject meleeEnemy;
    void Awake()
    {
        anim=GetComponent<Animator>();
        anim.SetBool("SmokeFinish",true);
    }
    public void SpawnEnemy()
    {
        if (meleeEnemy == null)
        {
            return;
        }

        Instantiate(meleeEnemy, transform.position, Quaternion.identity);

    }

    public void DestroyCloud()
    {
        Destroy(gameObject);
    }
}
