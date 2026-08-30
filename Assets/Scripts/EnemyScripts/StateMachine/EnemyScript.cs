using System;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{   


    public int health =100;
    public float moveSpeed;
    [SerializeField]Transform groundCheck;
    [SerializeField] Transform wallCheck;
    [SerializeField] Transform playerCheck;
    [SerializeField]float groundCheckDistance;
    [SerializeField] float wallCheckDistance;
    [SerializeField] float playerCheckDistance;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] LayerMask whatIsPlayer;

    public Animator anim;
    public Rigidbody2D rb;

    public float timer;
    public float timeToRun;
    public float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 0.6f;
    [SerializeField] private Transform attackHitPoint;
    [SerializeField] private float attackHitRadius = 0.7f;
    [SerializeField] private int attack1Damage = 10;
    [SerializeField] private int attack2Damage = 15;
    [SerializeField] private float getHitDuration = 0.3f;
    [SerializeField] private float getHitTimer;
    [SerializeField] private float hitKnockbackForce = 4f;
    private float nextAttackTime;
    public Transform player;

    public int facDir=1;
    public bool isFacingRight=true;

    public bool isGrounded;
    public bool isWallDetected;
    public bool isSeeingPlayer;
    
    
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

    public EnemyDeath death{get;private set;}
    public bool attackFinish=false;
    public bool attackFinish2;
    public bool dealingDamage=false;
    public bool getHitFinish=false;

    void Awake()
    {   anim= GetComponentInChildren<Animator>();
        rb= GetComponent<Rigidbody2D>();
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

        if (enemyStateMachine.currentState == getHit)
        {
            enemyStateMachine.currentState.Update();
            return;
        }

        bool isAttacking = enemyStateMachine.currentState == enemyAttack1 || enemyStateMachine.currentState == enemyAttack2;

        if (!isAttacking)
        {
            bool isInCombatPhase = enemyStateMachine.currentState == battleState || isSeeingPlayer;
            if (isInCombatPhase)
            {
                GeneralFlipCheck();
            }
        }

        if ((enemyStateMachine.currentState != enemyAttack1 && enemyStateMachine.currentState != enemyAttack2) && isSeeingPlayer && enemyStateMachine.currentState != battleState)
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
        isWallDetected= Physics2D.Raycast(wallCheck.position,Vector2.right,wallCheckDistance*facDir,whatIsGround);
        isSeeingPlayer= Physics2D.Raycast(wallCheck.position,Vector2.right*facDir,playerCheckDistance,whatIsPlayer);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance, 0));
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + new Vector3(facDir * wallCheckDistance, 0, 0));
        Gizmos.DrawLine(playerCheck.position, playerCheck.position + new Vector3(facDir * playerCheckDistance, 0, 0));

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
                int damage = enemyAttack1 != null && enemyStateMachine.currentState == enemyAttack1 ? attack1Damage : attack2Damage;
                playerScript.TakeDamage(this, damage);
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
    }

    public void AttackFinish()
    {
        attackFinish = true;
    }

    public void Attack2Finish()
    {
        attackFinish2 = true;
    }

    public void HitByPlayer()
    {
        if (enemyStateMachine == null || getHit == null || player == null)
        {
            return;
        }

        getHitTimer = getHitDuration;
        getHitFinish = false;

        Vector2 knockbackDirection = (transform.position - player.position).normalized;
        if (knockbackDirection == Vector2.zero)
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
