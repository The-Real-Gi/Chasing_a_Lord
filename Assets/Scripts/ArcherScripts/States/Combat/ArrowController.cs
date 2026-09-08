using UnityEngine;

public class ArrowController : MonoBehaviour
{
    Rigidbody2D rb;
    public int facDir;
    public float moveDir;
    [SerializeField] private int arrowDamage = 10;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float arrowLifetime = 5f;
    [SerializeField] private float fadeDuration = 1f;
    private SpriteRenderer[] spriteRenderers;
    private float[] originalAlphas;
    private float lifetimeTimer;
    private bool isFrozen;
    private bool isStuckToPlayer;
    private PlayerScript stuckPlayer;
    void Awake()
    {
        rb =GetComponent<Rigidbody2D>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        originalAlphas = new float[spriteRenderers.Length];
        for (int index = 0; index < spriteRenderers.Length; index++)
        {
            originalAlphas[index] = spriteRenderers[index].color.a;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lifetimeTimer += Time.deltaTime;
        float fadeStartTime = Mathf.Max(0f, arrowLifetime - fadeDuration);
        if (lifetimeTimer >= fadeStartTime)
        {
            float fadeProgress = Mathf.InverseLerp(fadeStartTime, arrowLifetime, lifetimeTimer);
            Fade(fadeProgress);
        }

        if (lifetimeTimer >= arrowLifetime)
        {
            Destroy(gameObject);
            return;
        }

        if (isStuckToPlayer && stuckPlayer != null && stuckPlayer.health <= 0f)
        {
            DropFromPlayer();
        }
    }

    private void Fade(float progress)
    {
        for (int index = 0; index < spriteRenderers.Length; index++)
        {
            Color color = spriteRenderers[index].color;
            color.a = Mathf.Lerp(originalAlphas[index], 0f, progress);
            spriteRenderers[index].color = color;
        }
    }
    void FixedUpdate()
    {
        if (!isFrozen)
        {
            MoveInSetDirection();
        }

    }
    public void SetFacDir(int dir)
    {
        facDir = dir >= 0 ? 1 : -1;
    }

    public void SetDirection(int dir, float angle, float speed)
    {
        SetFacDir(dir);
        moveDir = angle;
        moveSpeed = speed;
        isFrozen = false;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.WakeUp();

        MoveInSetDirection();
    }

    public void IgnoreCollisionWith(GameObject other)
    {
        if (other == null)
        {
            return;
        }

        foreach (Collider2D arrowCollider in GetComponentsInChildren<Collider2D>())
        {
            foreach (Collider2D otherCollider in other.GetComponentsInChildren<Collider2D>())
            {
                Physics2D.IgnoreCollision(arrowCollider, otherCollider);
            }
        }
    }

    private void MoveInSetDirection()
    {
        float angleInRadians = moveDir * Mathf.Deg2Rad;
        Vector2 velocity = new Vector2(
            Mathf.Cos(angleInRadians) * facDir,
            Mathf.Sin(angleInRadians)
        ) * moveSpeed;

        rb.linearVelocity = velocity;
        transform.right = velocity.normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerScript player = collision.gameObject.GetComponentInParent<PlayerScript>();
        if (player != null && player.IsBlocking() && !isStuckToPlayer)
        {
            BlockedByPlayer(player);
            return;
        }

        if (player != null && player.health > 0f && !isStuckToPlayer)
        {
            player.TakeDamage(arrowDamage, transform);
            StickToPlayer(player);
            return;
        }

        if (!isStuckToPlayer && (whatIsGround.value & (1 << collision.gameObject.layer)) != 0)
        {
            isFrozen = true;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void BlockedByPlayer(PlayerScript player)
    {
        if (player.rb != null)
        {
            player.rb.linearVelocity = Vector2.zero;
        }

        foreach (Collider2D arrowCollider in GetComponentsInChildren<Collider2D>())
        {
            foreach (Collider2D playerCollider in player.GetComponentsInChildren<Collider2D>())
            {
                Physics2D.IgnoreCollision(arrowCollider, playerCollider);
            }
        }

        DropFromPlayer();
    }

    private void StickToPlayer(PlayerScript player)
    {
        isStuckToPlayer = true;
        stuckPlayer = player;
        isFrozen = true;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.SetParent(player.transform, true);

        foreach (Collider2D arrowCollider in GetComponentsInChildren<Collider2D>())
        {
            arrowCollider.enabled = false;
        }

        rb.simulated = false;
    }

    private void DropFromPlayer()
    {
        isStuckToPlayer = false;
        isFrozen = true;
        moveSpeed = 0f;
        transform.SetParent(null, true);

        foreach (Collider2D arrowCollider in GetComponentsInChildren<Collider2D>())
        {
            arrowCollider.enabled = true;
        }

        rb.simulated = true;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.WakeUp();
        stuckPlayer = null;
    }

}
