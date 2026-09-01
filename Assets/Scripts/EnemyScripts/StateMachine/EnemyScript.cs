using System;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{   


    public int health =100;
    public float moveSpeed;
    [SerializeField]Transform groundCheck;
    [SerializeField] Transform wallCheck;
    [SerializeField] Transform playerCheck;
    [SerializeField] Transform playerCrouchCheck;
    [SerializeField] Transform forcedCrouchCheck;
    [SerializeField] Transform forcedCrouchCheck2;

    [SerializeField]float groundCheckDistance;
    [SerializeField] float wallCheckDistance;
    [SerializeField] float playerCheckDistance;
    [SerializeField] float playerCrouchCheckDistance;
    [SerializeField] float forcedCrouchCheckDistance;
    [SerializeField] float forcedCrouchCheckDistance2;
    [SerializeField] private float wallDetectionGraceDuration = 0.1f;
    [SerializeField] private float crouchObstacleGraceDuration = 0.2f;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] LayerMask whatIsPlayer;

    public Animator anim;
    public Rigidbody2D rb;
    public CapsuleCollider2D enemyCollider;
    public Vector2 crouchColliderSize;
    public Vector2 crouchColliderOffset;
    public Vector2 baseColliderSize;
    public Vector2 baseColliderOffset;

    public float timer;
    public float timeToRun;
    [SerializeField] private float crouchIdleMoveDelay = 2f;
    [SerializeField] private float crouchSearchDuration = 2f;
    public float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 0.6f;
    [SerializeField] private Transform attackHitPoint;
    [SerializeField] private float attackHitRadius = 0.7f;
    [SerializeField] private int attack1Damage = 10;
    [SerializeField] private int attack2Damage = 15;
    [SerializeField] private int attack3Damage= 5;
    [SerializeField] private float getHitDuration = 0.3f;
    [SerializeField] private float getHitTimer;
    [SerializeField] private float hitKnockbackForce = 4f;
    [SerializeField] public int hitsBeforeRoll = 2;
    [SerializeField] public float rollSpeed = 6f;
    [SerializeField] public float rollDuration = 0.7f;
    private int playerHitCounter;
    private float wallDetectedTimer;
    private float crouchObstacleTimer;
    private float nextAttackTime;
    public Transform player;

    public int facDir=1;
    public bool isFacingRight=true;

    public bool isGrounded;
    public bool isWallDetected;
    public bool isSeeingPlayer;
    public bool isSeeingPlayerCrouched;
    public bool isForcedCrouch;
    public bool isForcedCrouch2;
    
    
    EnemyStateMachine enemyStateMachine;
    public EnemyIdle enemyIdle{get;private set;}
    public EnemyMove enemyMove{get;private set;}
    public EnemyCrouchIdle enemyCrouchIdle {get;private set;}
    public EnemyCrouchMove enemyCrouchMove {get;private set;}
    public EnemyRoll enemyRoll {get;private set;}

    public EnemyBattleState battleState {get; private set;}
    public EnemyGetHit getHit {get;private set;}
    public EnemyAttack1 enemyAttack1 {get;private set;}
    public EnemyAttack2 enemyAttack2 {get;private set;}
    public EnemyCrouchAttack enemyCrouchAttack {get;private set;}

    public EnemyDeath death{get;private set;}
    public bool attackFinish=false;
    public bool attackFinish2;
    public bool attackFinish3 = false;
    public bool dealingDamage=false;
    public bool getHitFinish=false;


    void Awake()
    {   anim= GetComponentInChildren<Animator>();
        rb= GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<CapsuleCollider2D>();
        if (enemyCollider != null)
        {
            baseColliderSize = enemyCollider.size;
            baseColliderOffset = enemyCollider.offset;
        }
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        enemyStateMachine = new EnemyStateMachine();
        enemyIdle= new EnemyIdle(this,enemyStateMachine,"Idle");
        enemyMove= new EnemyMove(this,enemyStateMachine,"Move");
        battleState = new EnemyBattleState (this,enemyStateMachine,"Move");
        enemyCrouchIdle = new EnemyCrouchIdle(this,enemyStateMachine,"CrouchIdle");
        enemyCrouchMove = new EnemyCrouchMove(this,enemyStateMachine,"CrouchMove");
        enemyRoll= new EnemyRoll(this,enemyStateMachine,"Roll"); 
        enemyAttack1 = new EnemyAttack1(this,enemyStateMachine,"Attack1");
        enemyAttack2 = new EnemyAttack2(this,enemyStateMachine,"Attack2");
        enemyCrouchAttack = new EnemyCrouchAttack(this, enemyStateMachine, "CrouchAttack");
        getHit= new EnemyGetHit(this,enemyStateMachine,"GetHit");
        death = new EnemyDeath(this,enemyStateMachine,"Death");
        enemyStateMachine.Initialize(enemyIdle);
    
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Checks();

        bool isInCrouchState = enemyStateMachine.currentState == enemyCrouchIdle || enemyStateMachine.currentState == enemyCrouchMove;
        if (!isInCrouchState && MustCrouch())
        {
            enemyStateMachine.ChangeState(enemyCrouchIdle);
            return;
        }

        if (enemyStateMachine.currentState == getHit&&health>0)
        {
            enemyStateMachine.currentState.Update();
            return;
        }

        bool isAttacking = enemyStateMachine.currentState == enemyAttack1
            || enemyStateMachine.currentState == enemyAttack2
            || enemyStateMachine.currentState == enemyCrouchAttack;

        if (!isAttacking && enemyStateMachine.currentState != enemyRoll)
        {
            bool isInCombatPhase = enemyStateMachine.currentState == battleState
                || isSeeingPlayer
                || isSeeingPlayerCrouched;
            if (isInCombatPhase)
            {
                GeneralFlipCheck();
            }
        }

        // Crouch states own their transitions while the enemy is under a low
        // ceiling. Do not replace them with the standing battle state.
        bool canEnterBattleState = enemyStateMachine.currentState == enemyIdle
            || enemyStateMachine.currentState == enemyMove;

        if (CanSeePlayer() && canEnterBattleState)
        {
            enemyStateMachine.ChangeState(battleState);
        }

        enemyStateMachine.currentState.Update();
        if (health <= 0)
        {
            enemyStateMachine.ChangeState(death);
        }
    }

    private void GeneralFlipCheck()
    {
        if (player == null)
            return;

        float delta = player.position.x - transform.position.x;
        if (Mathf.Abs(delta) < 0.05f)
            return;

        facDir = delta > 0 ? 1 : -1;
        UpdateFacingFromDirection(facDir);
    }

    public bool CanSeePlayer()
    {
        return isSeeingPlayer || isSeeingPlayerCrouched;
    }

    public bool MustCrouch()
    {
        // Keep crouch pursuit stable when a wall ray flickers for a frame
        // while the enemy moves through a low passage.
        return crouchObstacleTimer > 0f;
    }

    void FixedUpdate()
    {
        if (enemyStateMachine.currentState == getHit)
        {
            enemyStateMachine.currentState.FixedUpdate();
            return;
        }

        enemyStateMachine.currentState.FixedUpdate();
    }

    void Checks()
    {
        isGrounded= Physics2D.Raycast(groundCheck.position,Vector2.down,groundCheckDistance,whatIsGround);
        bool wallDetectedThisFrame = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance * facDir, whatIsGround);
        if (wallDetectedThisFrame)
        {
            wallDetectedTimer = wallDetectionGraceDuration;
        }
        else
        {
            wallDetectedTimer = Mathf.Max(0f, wallDetectedTimer - Time.deltaTime);
        }

        isWallDetected = wallDetectedTimer > 0f;
        isSeeingPlayer= Physics2D.Raycast(wallCheck.position,Vector2.right*facDir,playerCheckDistance,whatIsPlayer);
        isSeeingPlayerCrouched = playerCrouchCheck != null && Physics2D.Raycast(playerCrouchCheck.position, Vector2.right * facDir, playerCrouchCheckDistance, whatIsPlayer);
        isForcedCrouch = forcedCrouchCheck != null && Physics2D.Raycast(forcedCrouchCheck.position, Vector2.up, forcedCrouchCheckDistance, whatIsGround);
        isForcedCrouch2 = forcedCrouchCheck2 != null && Physics2D.Raycast(forcedCrouchCheck2.position, Vector2.up, forcedCrouchCheckDistance2, whatIsGround);

        bool crouchObstacleDetected = isForcedCrouch || isForcedCrouch2 || (isWallDetected && CanSeePlayer());
        if (crouchObstacleDetected)
        {
            crouchObstacleTimer = crouchObstacleGraceDuration;
        }
        else
        {
            crouchObstacleTimer = Mathf.Max(0f, crouchObstacleTimer - Time.deltaTime);
        }

    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance, 0));
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + new Vector3(facDir * wallCheckDistance, 0, 0));
        Gizmos.DrawLine(playerCheck.position, playerCheck.position + new Vector3(facDir * playerCheckDistance, 0, 0));
        if (playerCrouchCheck != null)
        {
            Gizmos.DrawLine(playerCrouchCheck.position, playerCrouchCheck.position + new Vector3(facDir * playerCrouchCheckDistance, 0, 0));
        }
        if (forcedCrouchCheck != null)
        {
            Gizmos.DrawLine(forcedCrouchCheck.position, forcedCrouchCheck.position + new Vector3(0, forcedCrouchCheckDistance, 0));
        }
        if (forcedCrouchCheck2 != null)
        {
            Gizmos.DrawLine(forcedCrouchCheck2.position, forcedCrouchCheck2.position + new Vector3(0, forcedCrouchCheckDistance2, 0));
        }
        if (attackHitPoint != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(attackHitPoint.position, attackHitRadius);
        }
    }

    public void TryDealDamage()
    {
        if (attackHitPoint == null || player == null)
        {
            return;
        }

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackHitPoint.position, attackHitRadius, whatIsPlayer);

        foreach (Collider2D hitCollider in hitColliders)
        {
            PlayerScript playerScript = hitCollider.GetComponentInParent<PlayerScript>();

            if (playerScript != null)
            {
                int damage = enemyStateMachine.currentState == enemyAttack1 ? attack1Damage
                    : enemyStateMachine.currentState == enemyAttack2 || enemyStateMachine.currentState == enemyCrouchAttack ? attack2Damage
                    : attack3Damage;
                playerScript.TakeDamage(damage, transform);
                return;
            }
        }
    }

    public bool IsPlayerInAttackRange()
    {
        if (player == null)
        {
            return false;
        }

        return Vector2.Distance(transform.position, player.position) <= attackRange;
    }

    public void ApplyCrouchCollider()
    {
        if (enemyCollider == null)
        {
            return;
        }

        enemyCollider.size = crouchColliderSize;
        enemyCollider.offset = crouchColliderOffset;
    }

    public void ResetColliderToBase()
    {
        if (enemyCollider == null)
        {
            return;
        }

        enemyCollider.size = baseColliderSize;
        enemyCollider.offset = baseColliderOffset;
    }

    public float CrouchIdleMoveDelay => crouchIdleMoveDelay;
    public float CrouchSearchDuration => crouchSearchDuration;

    public void FlipController()
    {
        UpdateFacingFromDirection(facDir);
    }

    public void Flip()
    {
        facDir *= -1;
        UpdateFacingFromDirection(facDir);
    }

    private void UpdateFacingFromDirection(int direction)
    {
        isFacingRight = direction >= 0;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    public bool CanAttackAgain()
    {
        return Time.time >= nextAttackTime;
    }

    public void BeginAttack()
    {
        nextAttackTime = Time.time + attackCooldown;
        attackFinish = false;
        attackFinish2 = false;
        attackFinish3 = false;
    }

    public void AttackFinish()
    {
        attackFinish = true;
    }

    public void Attack2Finish()
    {
        attackFinish2 = true;
    }
    public void Attack3Finish()
    {
        attackFinish3 = true;
    }

    public void HitByPlayer()
    {
        if (enemyStateMachine == null || getHit == null || player == null || enemyStateMachine.currentState == enemyRoll)
        {
            return;
        }

        playerHitCounter++;

        // With hitsBeforeRoll set to 2, the second player hit starts the roll.
        if (playerHitCounter >= Mathf.Max(1, hitsBeforeRoll))
        {
            playerHitCounter = 0;

            facDir = player.position.x >= transform.position.x ? 1 : -1;
            UpdateFacingFromDirection(facDir);

            enemyStateMachine.ChangeState(enemyRoll);
            return;
        }

        getHitTimer = getHitDuration; //get hit timer starts and get hit finish is false
        getHitFinish = false;

        Vector2 knockbackDirection = (transform.position - player.position).normalized; // knockbackDirection is objects position minus player position normalised
        if (knockbackDirection == Vector2.zero) // if the result is 0 then its left
        {
            knockbackDirection = Vector2.left;
        }

        rb.linearVelocity = new Vector2(knockbackDirection.x * hitKnockbackForce, rb.linearVelocity.y);

        facDir = player.position.x >= transform.position.x ? 1 : -1;
        UpdateFacingFromDirection(facDir);

        enemyStateMachine.ChangeState(getHit);
    }

    public void UpdateGetHitState()
    {
        if (enemyStateMachine.currentState != getHit)
        {
            return;
        }

        getHitTimer -= Time.deltaTime;

        if (getHitTimer <= 0f)
        {
            getHitFinish = true;
            enemyStateMachine.ChangeState(enemyIdle);
        }
    }

    public void GettingHit()
    {
        getHitFinish = true;
    }
}
