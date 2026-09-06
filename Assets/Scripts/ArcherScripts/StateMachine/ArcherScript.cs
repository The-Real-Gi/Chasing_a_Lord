using System;
using UnityEngine;

public class ArcherScript : MonoBehaviour
{

    public float health=100;

    public Animator anim;
    public Rigidbody2D rb;

    [SerializeField] private float idleDuration = 2f;
    public float IdleDuration => idleDuration;

    public int facDir = 1;
    public bool isFacingRight = true;
    

    ArcherStateMachine stateMachine;
    public ArcherIdle idle {get;private set;}
    public ArcherMove move {get;private set;}
    public ArcherShoot shoot {get;private set;}
    public ArcherGetHit getHit {get;private set;}
    public ArcherDeath death {get;private set;}
    public ArcherBattleState battleState {get;private set;}

    public float moveSpeed = 3f;

    [SerializeField] private float getHitDuration = 0.35f;
    private float getHitTimer;
    [SerializeField] private float hitKnockbackForce = 5f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform groundCheck2;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask whatIsGround;
    public bool isGrounded;

    [SerializeField] private float playerDetectionRadius = 5f;
    [SerializeField] private float tooCloseRange = 2f;
    [SerializeField] private float shootingRange = 8f;
    [SerializeField] private Transform arrowSpawnPoint;
    [SerializeField] private float arrowSpeed = 10f;
    [SerializeField] private LayerMask whatIsPlayer;
    public bool shooting;

    public GameObject arrow;

    void Awake()
    {
        if (moveSpeed <= 0f)
        {
            moveSpeed = 3f;
        }

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        stateMachine = new ArcherStateMachine();
        idle = new ArcherIdle (this,stateMachine,"Idle");
        move = new ArcherMove (this,stateMachine,"Move");
        shoot = new ArcherShoot(this, stateMachine,"Shoot");
        getHit = new ArcherGetHit (this,stateMachine,"GetHit");
        death = new ArcherDeath(this,stateMachine,"Die");

        battleState= new ArcherBattleState(this,stateMachine,"Move");
        stateMachine.Initialize(idle);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0f)
        {
            if (stateMachine.currentState != death)
            {
                stateMachine.ChangeState(death);
            }
            return;
        }

        if (stateMachine.currentState == getHit)
        {
            stateMachine.currentState.Update();
            return;
        }

        bool isCombatState = stateMachine.currentState == battleState
            || stateMachine.currentState == move
            || stateMachine.currentState == shoot;
        if (isCombatState && !IsPlayerInRange())
        {
            stateMachine.ChangeState(idle);
            return;
        }

        stateMachine.currentState.Update();
        bool isPatrolling = stateMachine.currentState == idle;
        if (isPatrolling && IsPlayerInRange())
        {
            stateMachine.ChangeState(battleState);
        }
        Debug.Log(stateMachine.currentState);
    }

    void FixedUpdate()
    {
        Checks();
        stateMachine.currentState.FixedUpdate();
    }

    private void Checks()
    {
        bool firstGroundCheck = groundCheck != null
            && Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        bool secondGroundCheck = groundCheck2 != null
            && Physics2D.Raycast(groundCheck2.position, Vector2.down, groundCheckDistance, whatIsGround);
        isGrounded = firstGroundCheck && secondGroundCheck;
    }

    public void Flip()
    {
        facDir *= -1;
        isFacingRight = facDir >= 0;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * facDir;
        transform.localScale = scale;
    }

    public void TakeDamage(int damage, Transform attacker = null)
    {
        health -= damage;
        if (health > 0f && stateMachine.currentState != getHit)
        {
            HitByPlayer(attacker);
        }
    }

    public void HitByPlayer(Transform attacker = null)
    {
        if (stateMachine.currentState == death)
        {
            return;
        }

        getHitTimer = getHitDuration;
        Vector2 knockbackDirection = attacker == null
            ? Vector2.left
            : ((Vector2)transform.position - (Vector2)attacker.position).normalized;
        if (knockbackDirection == Vector2.zero)
        {
            knockbackDirection = Vector2.left;
        }

        rb.linearVelocity = new Vector2(knockbackDirection.x * hitKnockbackForce, rb.linearVelocity.y);
        stateMachine.ChangeState(getHit);
    }

    public void UpdateGetHitState()
    {
        getHitTimer -= Time.deltaTime;
        if (getHitTimer <= 0f)
        {
            stateMachine.ChangeState(idle);
        }
    }

    public bool IsPlayerInRange()
    {
        return FindPlayerInRange(playerDetectionRadius) != null;
    }

    public bool IsPlayerInShootingRange()
    {
        return FindPlayerInRange(shootingRange) != null;
    }

    public bool IsPlayerTooClose()
    {
        return FindPlayerInRange(tooCloseRange) != null;
    }

    public void FacePlayer()
    {
        Transform player = FindPlayerInRange(playerDetectionRadius);
        if (player == null)
        {
            return;
        }

        float horizontalDistance = player.position.x - transform.position.x;
        if (Mathf.Abs(horizontalDistance) > 0.05f && Mathf.Sign(horizontalDistance) != facDir)
        {
            Flip();
        }
    }

    public void FaceAwayFromPlayer()
    {
        Transform player = FindPlayerInRange(playerDetectionRadius);
        if (player == null)
        {
            return;
        }

        float horizontalDistance = transform.position.x - player.position.x;
        int awayDirection = Mathf.Abs(horizontalDistance) <= 0.05f
            ? facDir
            : horizontalDistance > 0f ? 1 : -1;

        if (awayDirection != facDir)
        {
            Flip();
        }
    }

    private Transform FindPlayerInRange(float radius)
    {
        Collider2D[] playerHits = Physics2D.OverlapCircleAll(transform.position, radius, whatIsPlayer);

        foreach (Collider2D playerHit in playerHits)
        {
            PlayerScript playerScript = playerHit.GetComponentInParent<PlayerScript>();
            if (playerScript != null && playerScript.health > 0f)
            {
                return playerScript.transform;
            }
        }

        return null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = IsPlayerInRange() ? Color.green : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);

        Gizmos.color = IsPlayerInShootingRange() ? Color.blue : Color.white;
        Gizmos.DrawWireSphere(transform.position, shootingRange);

        Gizmos.color = IsPlayerTooClose() ? Color.red : Color.gray;
        Gizmos.DrawWireSphere(transform.position, tooCloseRange);

        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        }
        if (groundCheck2 != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawLine(groundCheck2.position, groundCheck2.position + Vector3.down * groundCheckDistance);
        }
    }

    public void ShootSingal()
    {
        if (IsPlayerInShootingRange())
        {
            shooting = true;
        }
    }

    public void ShootArrow()
    {
        if (!shooting || arrow == null)
        {
            return;
        }

        shooting = false;
        Transform player = FindPlayerInRange(shootingRange);
        if (player == null)
        {
            return;
        }

        Transform spawnPoint = arrowSpawnPoint != null ? arrowSpawnPoint : transform;
        Vector2 direction = (Vector2)player.position - (Vector2)spawnPoint.position;
        if (IsPlayerTooClose())
        {
            Vector2 playerDirection = (Vector2)player.position - (Vector2)transform.position;
            if (playerDirection.sqrMagnitude > 0.0001f)
            {
                direction = playerDirection;
            }
        }

        if (direction.sqrMagnitude <= 0.0001f)
        {
            direction = Vector2.right * (facDir == 0 ? 1 : facDir);
        }

        int directionSign = direction.x >= 0f ? 1 : -1;
        float angle = Mathf.Atan2(direction.y, Mathf.Abs(direction.x)) * Mathf.Rad2Deg;

        GameObject spawnedArrow = Instantiate(arrow, spawnPoint.position, Quaternion.identity);
        ArrowController arrowController = spawnedArrow.GetComponent<ArrowController>();
        if (arrowController != null)
        {
            arrowController.SetDirection(directionSign, angle, arrowSpeed);
        }
    }
}
