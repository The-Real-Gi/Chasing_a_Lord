using Unity.VisualScripting;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerScript player =collision.gameObject.GetComponent<PlayerScript>();
            player.health+=20;
            Debug.Log("HEalth added");
            if(player.health>100)
            {
                player.health=100;
            }
            Destroy(this.gameObject);
        }      
        
    }
}
