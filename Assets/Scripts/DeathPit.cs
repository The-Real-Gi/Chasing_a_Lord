using UnityEngine;

public class DeathPit : MonoBehaviour
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
        if(collision.gameObject.tag=="Player")
        {   Debug.Log("Trying to kill player");
            collision.gameObject.GetComponent<PlayerScript>().health=-100;
            GameManager.Instance.DeathPanel();
        }
    }
}
