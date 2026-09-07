using UnityEngine;

public class SpawningMeleeEnemy : MonoBehaviour
{
    public GameObject meleeEnemy;
    Animator anim;

    void Awake()
    {
        anim= GetComponent<Animator>();
        anim.SetBool("SmokeFinish",true);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Destroy(this.gameObject);
    }
}
