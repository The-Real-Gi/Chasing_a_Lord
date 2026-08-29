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

    void Awake()
    {   anim= GetComponentInChildren<Animator>();
        rb= GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        enemyStateMachine = new EnemyStateMachine();
        enemyIdle= new EnemyIdle(this,enemyStateMachine,"Idle");
        enemyMove= new EnemyMove(this,enemyStateMachine,"Move");
        battleState = new EnemyBattleState (this,enemyStateMachine,"Move");
        enemyAttack1 = new EnemyAttack1(this,enemyStateMachine,"Attack1");

        enemyStateMachine.Initialize(enemyIdle);
    
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Checks();

        if (player != null)
        {
            if (player.position.x > transform.position.x)
            {
                facDir = 1;
            }
            else if (player.position.x < transform.position.x)
            {
                facDir = -1;
            }

            FlipController();
        }

        if (isSeeingPlayer && enemyStateMachine.currentState != battleState)
        {
            enemyStateMachine.ChangeState(battleState);
        }

        enemyStateMachine.currentState.Update();
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
        if(isFacingRight&&facDir==-1)
        {
            Flip();
        }else if(!isFacingRight&&facDir==1)
        {
            Flip();
        }
    }

    public void Flip()
    {
        isFacingRight=!isFacingRight;
        facDir=facDir*(-1);
        transform.localScale= new Vector3(facDir*2,2,2);
    }
}
