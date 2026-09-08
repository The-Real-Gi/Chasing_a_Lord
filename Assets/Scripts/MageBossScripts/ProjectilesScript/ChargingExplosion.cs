using UnityEngine;
using System.Collections.Generic;

public class ChargingExplosion : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private int damage = 20;

    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D explosionCollider;
    private bool isExploding;
    private bool hasDamagedPlayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        explosionCollider = GetComponent<Collider2D>();

        if (explosionCollider != null)
        {
            explosionCollider.isTrigger = true;
        }

        if (explosionCollider != null && explosionCollider.IsTouchingLayers(whatIsGround))
        {
            return;
        }

        anim.SetBool("Charging", true);
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void GoToExplosion()
    {
        isExploding = true;
        anim.SetBool("Charging", false);
        anim.SetBool("Explosion", true);

        if (explosionCollider == null)
        {
            return;
        }

        ContactFilter2D contactFilter = new ContactFilter2D
        {
            useTriggers = true
        };
        List<Collider2D> overlappingColliders = new List<Collider2D>();
        Physics2D.OverlapCollider(explosionCollider, contactFilter, overlappingColliders);

        foreach (Collider2D overlappingCollider in overlappingColliders)
        {
            TryDamagePlayer(overlappingCollider);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (isExploding)
        {
            TryDamagePlayer(collider);
        }
    }

    private void TryDamagePlayer(Collider2D collider)
    {
        if (hasDamagedPlayer)
        {
            return;
        }

        PlayerScript player = collider.GetComponentInParent<PlayerScript>();
        if (player == null || player.IsBlocking())
        {
            return;
        }

        hasDamagedPlayer = true;
        player.TakeDamage(damage, transform);
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
