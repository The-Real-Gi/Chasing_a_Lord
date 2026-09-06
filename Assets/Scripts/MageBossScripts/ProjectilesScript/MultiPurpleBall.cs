using UnityEngine;

public class MultiPurpleBall : MonoBehaviour
{
    public Vector2 moveDirection;
    public float moveSpeed;
    public int damage;

    Rigidbody2D rb;
    Animator anim;

    bool hasHitPlayer;

    void Awake()
    {
        rb= GetComponent<Rigidbody2D>();
        anim= GetComponent<Animator>();
        anim.SetBool("IsFlying",true);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerScript player = collision.gameObject.GetComponentInParent<PlayerScript>();
        if (hasHitPlayer)
        {
            return;
        }

        if (player != null)
        {
            hasHitPlayer = true;

            if (!IsBlockedByPlayer(player))
            {
                player.TakeDamage(damage, transform);
            }
        }

        rb.linearVelocity = Vector2.zero;
        anim.SetBool("IsFlying", false);
        anim.SetBool("BlowUp", true);
    }

     public void SetUp(Vector2 _moveDirection)
    {
        moveDirection = _moveDirection.normalized;
        rb.AddForce(moveDirection * moveSpeed, ForceMode2D.Impulse);
    }

    private bool IsBlockedByPlayer(PlayerScript player)
    {
        if (!player.IsBlocking())
        {
            return false;
        }

        float projectileSide = Mathf.Sign(transform.position.x - player.transform.position.x);
        if (projectileSide == 0f)
        {
            projectileSide = Mathf.Sign(-moveDirection.x);
        }

        return projectileSide == player.facDir;
    }

     public void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
