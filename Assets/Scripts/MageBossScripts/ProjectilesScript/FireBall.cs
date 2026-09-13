using UnityEngine;

public class FireBall : MonoBehaviour
{   

    public Vector2 moveDirection;
    public float moveSpeed = 8f;
    public int damage = 20;
    Rigidbody2D rb;
    Animator anim;
    bool hasHitPlayer;

    void Awake()
    {
        rb= GetComponent<Rigidbody2D>();
        anim=GetComponent<Animator>();
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
        rb.linearVelocity = moveDirection * moveSpeed;

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
        SetRotationFromMoveDirection();
    }

    private void SetRotationFromMoveDirection()
    {
        if (moveDirection.sqrMagnitude == 0f)
        {
            return;
        }

        float zRotation = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
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
