using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float moveDuration = 2f;

    Rigidbody2D rb;
    float moveTimer;
    float moveDirection = 1f;
    readonly HashSet<PlayerScript> playersTouching = new();
    public bool movingHorizontaly;
    public bool movingVertically;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        if(movingHorizontaly){
        moveTimer += Time.fixedDeltaTime;

        if (moveTimer >= moveDuration)
        {
            moveTimer = 0f;
            moveDirection *= -1f;
        }

        rb.linearVelocity = new Vector2(moveDirection * moveSpeed, rb.linearVelocity.y);

        foreach (PlayerScript player in playersTouching)
        {
            player.rb.position += Vector2.right * rb.linearVelocity.x * Time.fixedDeltaTime;
        }}
        if(movingVertically)
        {
        moveTimer += Time.fixedDeltaTime;

        if (moveTimer >= moveDuration)
        {
            moveTimer = 0f;
            moveDirection *= -1f;
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, moveDirection * moveSpeed);

        foreach (PlayerScript player in playersTouching)
        {
            player.rb.position += Vector2.right * rb.linearVelocity.x * Time.fixedDeltaTime;
        }
            
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerScript player = collision.collider.GetComponentInParent<PlayerScript>();
        if (player != null)
        {
            playersTouching.Add(player);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        PlayerScript player = collision.collider.GetComponentInParent<PlayerScript>();
        if (player != null)
        {
            playersTouching.Remove(player);
        }
    }
}
