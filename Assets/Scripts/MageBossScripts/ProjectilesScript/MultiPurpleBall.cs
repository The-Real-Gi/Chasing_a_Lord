using UnityEngine;

public class MultiPurpleBall : MonoBehaviour
{
    public Vector2 moveDirection;
    public float moveSpeed;
    public int damage;

    Rigidbody2D rb;
    Animator anim;

    bool hasImpacted;
    bool isFalling;
    float previousVelocityY;
    float currentZRotation;
    float fallingTargetZRotation;

    const float InitialZRotation = -105f;
    const float LeftwardFallingZRotation = 84f;
    const float RightwardFallingZRotation = -283f;
    const float FallingRotationSpeed = 180f;

    void Awake()
    {
        rb= GetComponent<Rigidbody2D>();
        anim= GetComponent<Animator>();
        currentZRotation = InitialZRotation;
        transform.rotation = Quaternion.Euler(0f, 0f, currentZRotation);
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
        float currentVelocityY = rb.linearVelocity.y;

        if (!isFalling && previousVelocityY > 0f && currentVelocityY <= 0f)
        {
            isFalling = true;
            fallingTargetZRotation = rb.linearVelocity.x < 0f
                ? LeftwardFallingZRotation
                : RightwardFallingZRotation;
        }

        if (isFalling)
        {
            currentZRotation = Mathf.MoveTowards(
                currentZRotation,
                fallingTargetZRotation,
                FallingRotationSpeed * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Euler(0f, 0f, currentZRotation);
        }

        previousVelocityY = currentVelocityY;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasImpacted)
        {
            return;
        }

        hasImpacted = true;
        PlayerScript player = collision.gameObject.GetComponentInParent<PlayerScript>();

        if (player != null)
        {
            if (!IsBlockedByPlayer(player))
            {
                player.TakeDamage(damage, transform);
            }
        }

        rb.linearVelocity = Vector2.zero;
        anim.SetBool("IsFlying", false);
        anim.SetBool("BlowUp", true);
        Destroy(gameObject, 0.65f);
    }

    public void SetUp(Vector2 _moveDirection)
    {
        moveDirection = _moveDirection.normalized;
        currentZRotation = InitialZRotation;
        transform.rotation = Quaternion.Euler(0f, 0f, currentZRotation);
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
