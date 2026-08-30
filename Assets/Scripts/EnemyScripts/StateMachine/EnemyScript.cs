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
    public EnemyBattleState battleState {get; private set;}
    public EnemyAttack1 enemyAttack1 {get;private set;}
    public EnemyDeath death{get;private set;}
    public bool attackFinish=false;
    public bool dealingDamage=false;

    void Awake()
    {   anim= GetComponentInChildren<Animator>();
        rb= GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        enemyStateMachine = new EnemyStateMachine();
        enemyIdle= new EnemyIdle(this,enemyStateMachine,"Idle");
        enemyMove= new EnemyMove(this,enemyStateMachine,"Move");
        battleState = new EnemyBattleState (this,enemyStateMachine,"Move");
        enemyAttack1 = new EnemyAttack1(this,enemyStateMachine,"Attack1");
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

        bool isAttacking = enemyStateMachine.currentState == enemyAttack1;

        if (!isAttacking)
        {
            bool isInCombatPhase = enemyStateMachine.currentState == battleState || isSeeingPlayer;
            if (isInCombatPhase)
            {
                GeneralFlipCheck();
            }
        }

        if (enemyStateMachine.currentState != enemyAttack1 && isSeeingPlayer && enemyStateMachine.currentState != battleState)
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
    }

    public void AttackFinish()
    {
        attackFinish=true;
    }
    public void DealingDamage()
    {
        
    }
}
