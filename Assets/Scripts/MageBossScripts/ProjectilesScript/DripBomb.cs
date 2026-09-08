using UnityEngine;

public class DripBomb : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 3f;

    private Rigidbody2D rb;
    private Animator anim;
    private bool hasExploded;
    public int damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetBool("IsFlying",true);
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.down * fallSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasExploded)
        {
            return;
        }

        hasExploded = true;

        PlayerScript player = collision.gameObject.GetComponentInParent<PlayerScript>();
        if (player != null && !player.IsBlocking())
        {
            player.TakeDamage(damage, transform);
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        anim.SetBool("IsFlying", false);
        anim.SetBool("Explosion", true);
    }

    public void DestroyObj()
    {
        Destroy(this.gameObject);
    }
}
