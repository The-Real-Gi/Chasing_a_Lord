using UnityEngine;

public class BossMageScript : MonoBehaviour
{
    private const float FacingDeadZone = 0.1f;

    public Animator anim;
    public Rigidbody2D rb;

    public float health = 300;

    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance = 0.5f;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float playerDetectionRadius = 2f;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;

    [SerializeField] private float idleDuration = 2f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float getHitDuration = 0.35f;
    private float attackCooldownTimer;
    private float getHitTimer;
    public float IdleDuration => idleDuration;
    public float MoveSpeed => moveSpeed;
    public bool CanAttack => attackCooldownTimer <= 0f;

    public int facDir = 1;
    public bool isFacingRight = true;
    public bool isWallDetected { get; private set; }
    public bool isPlayerSeen { get; private set; }
    private Transform playerTarget;
    
    public MageStateMachine stateMachine;
    public MageMove mageMove {get; private set;}
    public MageIdle mageIdle {get;private set;}
    public MageDeath mageDeath {get; private set;}
    public MageGetHit mageGetHit {get; private set;}

    public MageBattleState mageBattleState {get;private set;}
    public MageAttack1 mageAttack1 {get;private set;}
    public MageAttack2 mageAttack2 {get;private set;}
    public bool attackEnded=false;

    public bool spawnObj = false;

    public GameObject dropBombSpawnPos;

    public GameObject fireball;
    public GameObject multiPurpleBall;
    public GameObject droppingBomb;
    public GameObject explosion;
    public GameObject SpawningMeleeEnemy;
    public Transform multiPurpleBallSpawnPoint;
    public Transform meleeEnemySpawnPoint;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        stateMachine = new MageStateMachine();
        mageMove = new MageMove(this, stateMachine, "Move");
        mageIdle = new MageIdle(this, stateMachine, "Idle");
        mageBattleState = new MageBattleState(this,stateMachine,"Idle");
        mageDeath = new MageDeath(this,stateMachine,"Death");
        mageGetHit = new MageGetHit(this, stateMachine, "GetHit");
        mageAttack1 = new MageAttack1 (this,stateMachine,"Attack1");
        mageAttack2 = new MageAttack2(this,stateMachine,"Attack2");
        stateMachine.Initialize(mageIdle);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        attackCooldownTimer = Mathf.Max(0f, attackCooldownTimer - Time.deltaTime);

        if (health <= 0f)
        {
            health = 0f;
            if (stateMachine.currentState != mageDeath)
            {
                stateMachine.ChangeState(mageDeath);
            }

            stateMachine.currentState.Update();
            return;
        }

        CheckWall();
        CheckPlayerSeen();

        if (isPlayerSeen)
        {
            FacePlayer();
        }

        if (stateMachine.currentState == mageGetHit)
        {
            stateMachine.currentState.Update();
            return;
        }

        if (!isPlayerSeen && (stateMachine.currentState == mageBattleState
            || stateMachine.currentState == mageAttack1
            || stateMachine.currentState == mageAttack2))
        {
            stateMachine.ChangeState(mageIdle);
            return;
        }

        if (isPlayerSeen && CanAttack && stateMachine.currentState != mageBattleState
            && stateMachine.currentState != mageGetHit
            && stateMachine.currentState != mageAttack1 && stateMachine.currentState != mageAttack2)
        {
            stateMachine.ChangeState(mageBattleState);
            return;
        }

        stateMachine.currentState.Update();

    }
    void FixedUpdate()
    {
        stateMachine.currentState.FixedUpdate();
    }

    private void CheckWall()
    {
        if (wallCheck == null)
        {
            isWallDetected = false;
            return;
        }

        isWallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * facDir, wallCheckDistance, whatIsGround);
    }

    private void CheckPlayerSeen()
    {
        if (playerCheck == null)
        {
            isPlayerSeen = false;
            return;
        }

        Collider2D[] overlaps = Physics2D.OverlapCircleAll(playerCheck.position, playerDetectionRadius, whatIsPlayer);
        isPlayerSeen = false;
        playerTarget = null;

        foreach (Collider2D overlap in overlaps)
        {
            PlayerScript player = overlap != null ? overlap.GetComponentInParent<PlayerScript>() : null;
            if (player != null && player.health > 0f)
            {
                isPlayerSeen = true;
                playerTarget = player.transform;
                break;
            }
        }
    }

    public void FacePlayer()
    {
        if (playerTarget == null)
        {
            return;
        }

        float horizontalDirection = playerTarget.position.x - transform.position.x;
        if (Mathf.Abs(horizontalDirection) <= FacingDeadZone)
        {
            return;
        }

        int directionToPlayer = horizontalDirection > 0f ? 1 : -1;
        if (facDir != directionToPlayer)
        {
            Flip();
        }
    }

    public void Flip()
    {
        facDir *= -1;
        isFacingRight = facDir >= 0;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * facDir;
        transform.localScale = scale;
    }

    private void OnDrawGizmos()
    {
        if (wallCheck != null)
        {
            Gizmos.color = isWallDetected ? Color.red : Color.yellow;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + new Vector3(facDir * wallCheckDistance, 0f, 0f));
        }

        if (playerCheck != null)
        {
            Gizmos.color = isPlayerSeen ? Color.green : Color.yellow;
            Gizmos.DrawWireSphere(playerCheck.position, playerDetectionRadius);
        }
    }

    public void AttackFinisher()
    {
        attackEnded=true;
    }
    public void SpawnAttackObj()
    {
        spawnObj=true;
    }

    public void StartAttackCooldown()
    {
        attackCooldownTimer = attackCooldown;
    }

    public void TakeDamage(int damage, Transform attacker = null)
    {
        if (health <= 0f || stateMachine.currentState == mageDeath)
        {
            return;
        }

        health -= damage;
        if (health <= 0f)
        {
            health = 0f;
            stateMachine.ChangeState(mageDeath);
            return;
        }

        getHitTimer = getHitDuration;
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        stateMachine.ChangeState(mageGetHit);
    }

    public void UpdateGetHitState()
    {
        getHitTimer -= Time.deltaTime;
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (getHitTimer <= 0f)
        {
            stateMachine.ChangeState(isPlayerSeen ? mageBattleState : mageIdle);
        }
    }
}
