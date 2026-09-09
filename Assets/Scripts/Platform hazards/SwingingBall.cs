using UnityEngine;

public class SwingingBall : MonoBehaviour
{
    [SerializeField] float minimumSpeed = 10f;
    [SerializeField] float maximumSpeed = 180f;
    [SerializeField] float maximumMotorTorque = 1000f;
    [SerializeField] GameObject ball;//this is what will deal damage
    [SerializeField] int damage = 10;
    [SerializeField] float knockbackForce = 8f;
    [SerializeField] float verticalKnockback = 1f;

    HingeJoint2D hinge;
    Collider2D ballCollider;
    float swingDirection = 1f;

    void Awake()
    {
        hinge = GetComponent<HingeJoint2D>();
        if (ball != null)
        {
            ballCollider = ball.GetComponent<Collider2D>();
            if (ballCollider != null)
            {
                GameObject physicsHost = ballCollider.attachedRigidbody != null
                    ? ballCollider.attachedRigidbody.gameObject
                    : ball;
                BallDamageRelay relay = physicsHost.GetComponent<BallDamageRelay>();
                if (relay == null)
                {
                    relay = physicsHost.AddComponent<BallDamageRelay>();
                }

                relay.Initialize(this);
            }
        }else{
            Debug.Log("Ball is not theree");
        }

        hinge.useMotor = true;
        UpdateMotor();
    }

    void FixedUpdate()
    {
        float angle = hinge.jointAngle;
        float lowerLimit = hinge.limits.min;
        float upperLimit = hinge.limits.max;

        if (angle >= upperLimit - 1f)
        {
            swingDirection = -1f;
        }
        else if (angle <= lowerLimit + 1f)
        {
            swingDirection = 1f;
        }

        UpdateMotor();
    }

    void UpdateMotor()
    {
        float lowerLimit = hinge.limits.min;
        float upperLimit = hinge.limits.max;
        float normalizedAngle = Mathf.InverseLerp(lowerLimit, upperLimit, hinge.jointAngle);
        float speedCurve = Mathf.Sin(normalizedAngle * Mathf.PI);
        float speed = Mathf.Lerp(minimumSpeed, maximumSpeed, speedCurve);

        JointMotor2D motor = hinge.motor;
        motor.motorSpeed = swingDirection * speed;
        motor.maxMotorTorque = maximumMotorTorque;
        hinge.motor = motor;
    }

    public void DealDamage(Collider2D touchedCollider)
    {
        if (ballCollider == null || !ballCollider.IsTouching(touchedCollider))
        {
            return;
        }

        PlayerScript playerTarget = touchedCollider.GetComponentInParent<PlayerScript>();
        EnemyScript enemyTarget = touchedCollider.GetComponentInParent<EnemyScript>();
        ArcherScript archerTarget = touchedCollider.GetComponentInParent<ArcherScript>();

        if (playerTarget != null)
        {
            playerTarget.getHitKnockbackForce = knockbackForce;
            playerTarget.TakeDamage(damage, ball.transform);
        }
        else if (enemyTarget != null)
        {
            enemyTarget.health -= damage;
            enemyTarget.HitByPlayer(ball.transform);
            ApplyKnockback(enemyTarget.rb, enemyTarget.transform);
        }
        else if (archerTarget != null)
        {
            archerTarget.TakeDamage(damage, ball.transform);
            ApplyKnockback(archerTarget.rb, archerTarget.transform);
        }
    }

    void ApplyKnockback(Rigidbody2D targetRigidbody, Transform target)
    {
        if (targetRigidbody == null)
        {
            return;
        }

        float horizontalDirection = Mathf.Sign(target.position.x - ball.transform.position.x);
        if (horizontalDirection == 0f)
        {
            horizontalDirection = swingDirection;
        }

        targetRigidbody.linearVelocity = new Vector2(
            horizontalDirection * knockbackForce,
            verticalKnockback);
    }
}

public class BallDamageRelay : MonoBehaviour
{
    SwingingBall swingingBall;

    public void Initialize(SwingingBall source)
    {
        swingingBall = source;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (swingingBall != null)
        {
            swingingBall.DealDamage(collision.collider);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (swingingBall != null)
        {
            swingingBall.DealDamage(other);
        }
    }
}
