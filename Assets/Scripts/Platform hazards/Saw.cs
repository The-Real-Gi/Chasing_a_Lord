using UnityEngine;
using System.Collections.Generic;

public class Saw : MonoBehaviour
{
    [SerializeField] private Vector3 rotationSpeed = new Vector3(0f, 0f, 90f);
    [SerializeField] private bool moveVertically;
    [SerializeField] private bool moveHorizontally;
    [SerializeField] private bool moveInCircle;
    [SerializeField] private float movementDistance = 2f;
    [SerializeField] private float movementDuration = 2f;
    [SerializeField] private float circleRadius = 2f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float knockbackForce = 5f;

    private Vector3 startingLocalPosition;
    private float movementTimer;
    private readonly HashSet<GameObject> damagedTargets = new();

    void Start()
    {
        startingLocalPosition = transform.localPosition;
    }

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime, Space.Self);

        movementTimer += Time.deltaTime;

        if (moveInCircle)
        {
            float circleAngle = movementTimer / Mathf.Max(movementDuration, 0.01f) * Mathf.PI * 2f;
            Vector3 circleOffset = new Vector3(Mathf.Cos(circleAngle), Mathf.Sin(circleAngle), 0f) * circleRadius;
            transform.localPosition = startingLocalPosition + circleOffset;
            return;
        }

        float movementOffset = Mathf.PingPong(movementTimer, Mathf.Max(movementDuration, 0.01f)) /
            Mathf.Max(movementDuration, 0.01f) * 2f - 1f;
        Vector3 linearOffset = Vector3.zero;

        if (moveVertically)
        {
            linearOffset.y = movementOffset * movementDistance;
        }

        if (moveHorizontally)
        {
            linearOffset.x = movementOffset * movementDistance;
        }

        transform.localPosition = startingLocalPosition + linearOffset;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DealDamage(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DealDamage(other);
    }

    private void DealDamage(Collider2D targetCollider)
    {
        PlayerScript player = targetCollider.GetComponentInParent<PlayerScript>();
        EnemyScript enemy = targetCollider.GetComponentInParent<EnemyScript>();
        ArcherScript archer = targetCollider.GetComponentInParent<ArcherScript>();
        GameObject target = player != null ? player.gameObject : enemy != null ? enemy.gameObject : archer != null ? archer.gameObject : null;

        if (target == null || !damagedTargets.Add(target))
        {
            return;
        }

        if (player != null)
        {
            player.TakeDamage(damage, transform);
            ApplyKnockback(player.rb, target.transform);
        }
        else if (enemy != null)
        {
            if (enemy.TakeDamage(damage, transform))
            {
                ApplyKnockback(enemy.rb, target.transform);
            }
        }
        else
        {
            archer.TakeDamage(damage, transform);
            ApplyKnockback(archer.rb, target.transform);
        }
    }

    private void ApplyKnockback(Rigidbody2D targetRigidbody, Transform target)
    {
        if (targetRigidbody == null)
        {
            return;
        }

        Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
        if (direction == Vector2.zero)
        {
            direction = Vector2.right;
        }

        targetRigidbody.linearVelocity = new Vector2(direction.x * knockbackForce*5, knockbackForce);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        RemoveTarget(collision.collider);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        RemoveTarget(other);
    }

    private void RemoveTarget(Collider2D targetCollider)
    {
        PlayerScript player = targetCollider.GetComponentInParent<PlayerScript>();
        EnemyScript enemy = targetCollider.GetComponentInParent<EnemyScript>();
        ArcherScript archer = targetCollider.GetComponentInParent<ArcherScript>();

        if (player != null)
        {
            damagedTargets.Remove(player.gameObject);
        }
        else if (enemy != null)
        {
            damagedTargets.Remove(enemy.gameObject);
        }
        else if (archer != null)
        {
            damagedTargets.Remove(archer.gameObject);
        }
    }
}
