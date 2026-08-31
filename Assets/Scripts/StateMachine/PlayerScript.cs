using System.Collections.Generic;
using Unity.Android.Gradle;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public Animator anim;
    [HideInInspector]
    public Rigidbody2D rb;

    public StateMachine stateMachine;
    #region States
    public IdleState idle{get;private set;}
    public MoveState move {get;private set;}
    public JumpState jump{get;private set;}
    public AirState airState{get;private set;}
    public WallSlide wallSlide{get;private set;}
    public WallJump wallJump {get;private set;}
    public LedgeClimbState ledgeClimbState{get;private set;}
    public WallHangState wallHangState {get;private set;}
    public CrouchIdle crouchIdle {get;private set;}
    public CrouchMove crouchMove {get;private set;}
    public DashState dashState{get;private set;}
    public SlideState slideState{get;private set;}
    public DoubleJump doubleJump{get;private set;}


    public ShieldBlock shieldBlock {get;private set;}
    public CrouchBlock crouchBlock {get;private set;}

    public GetHit getHit{get;private set;}
    public PlayerDeath playerDeath{get;private set;}

    public MeleeAtt1 meleeAtt1{get;private set;}
    public MeleeAtt2 meleeAtt2{get;private set;}
    public Kick kick{get;private set;}
    public MeleeRun meleeRun{get;private set;}
    public MeleeSpin meleeSpin{get;private set;}
    public CrouchAttack crouchAttack {get;private set;}

    public bool isAttacking=false;


    #endregion

    public Vector2 inputVector;

    #region Flip
    public bool isFacingRight=true;
    public int facDir=1;
    #endregion

    #region Stats

    public float health;
    public float moveSpeed;
    public float jumpStrength;
    public float wallJumpStrength;
    #endregion
    
    #region Checks
    public Transform groundCheck;
    public float groundCheckDistance;
    public LayerMask whatIsGround;
    public Transform wallCheck;
    public float wallCheckDistance;
    public Transform hangCheck;
    public float hangCheckDistance;
    public Transform forcedCrouchCheck;
    public float forcedCrouchCheckDistance;
    public bool isGrounded;
    public bool isWallDetected;
    public bool isWallJumping=false;
    public bool isHandging=false;
    public bool isTouchingLedge=false;
    public bool ledgeDetected;
    public bool canClimbLedge=false;
    public bool isForcedCrouchDetected;

    #endregion

    #region Ledges
    public Vector2 ledgePosBot;
    public Vector2 ledgePos1;
    public  Vector2 ledgePos2;
    public float ledgeClimbXOffset1 = 0f;
    public float ledgeClimbYOffset1 = 0f;
    public float ledgeClimbXoffset2 = 0f;
    public float ledgeclimbYOffset2 = 0f;
    public bool finishedCLimb=false;
    public bool moveUp=false;
    public bool moveForward=false;
    public float climbingUpSpeed;
    public float movingForwardSpeed;
    public bool isClimbingLedge=false;
    #endregion
    
    public float timer;
    public float dashCooldown;
    public float dashingTime;
    public float dashSpeed;
    public float cooldownTimer;
    public float flipCooldown = 1f;
    public float flipCooldownTimer;

    public float maxRunSpeed;
    public float baseRunSpeed;

    public PlayersInputSet input;
  
    public int jumpCount;
    public int maxJumps;
    public Vector2 crouchColliderOffset;
    public Vector2 baseCollider;
    public Vector2 sizeCollider;
    public CapsuleCollider2D playerCollider;
    public Vector2 collidersizeCrouch;

    #region Combat
    public bool finishAttack=false;
    public bool kickDamageDealt=false;
    public float kickMoveSpeed = 5f;
    public float meleeAtt1MoveSpeed = 5f;
    public float meleeAtt2MoveSpeed = 6f;
    [SerializeField] public float getHitDuration = 0.35f;
    [SerializeField] public float getHitTimer = 0f;
    [SerializeField] public float getHitKnockbackForce ;
    [HideInInspector] public float getHitKnockbackDirection = 1f;
    public float meleeAttackBlendSpeed = 8f;
    public float meleeSpinSlowdownRate = 0.08f;
    public float meleeRunAccelerationSpeed = 0.12f;
    public float meleeRunAccelerationTimer = 0.25f;
    public float meleeSpinPushForce ;
    public float meleeSpinUpForce ;
    public bool meleeSpinDamageDealt=false;
    public bool meleeAtt1DamageDealt=false;
    public bool meleeAtt2DamageDealt=false;
    public bool crouchMeleeDamageDealt=false;
    public GameObject meleeAtt1AttackPos1;
    public GameObject meleeAtt2AttackPos2;
    public GameObject meleeRunAttackPos3;
    public GameObject meleeSpinAttackPos4;
    public GameObject meleeAttKickAttackPos5;
    public GameObject meleeAttCrouchAttackPos6;
    public float attack1Distance1;
    public float attack2Distance;
    public float attack3Distance;
    public float attack4Distance;
    public float attack5Distance;
    public float attack6Distance;
    public LayerMask whatIsEnemy;
    public List<EnemyScript> enemiesInAttackRange = new List<EnemyScript>();
    public bool isForcedCrouch;

    #endregion


    

    void Awake()
    {   input = new PlayersInputSet();
        anim = GetComponentInChildren<Animator>();
        stateMachine = new StateMachine();
        rb = GetComponent<Rigidbody2D>();
        playerCollider= GetComponent<CapsuleCollider2D>();
        playerCollider.offset=baseCollider;
        sizeCollider=playerCollider.size;
        idle = new IdleState(this,"Idle",stateMachine);
        move = new MoveState(this,"Run",stateMachine);
        jump = new JumpState(this, "Jump",stateMachine);
        airState = new AirState(this,"Jump",stateMachine);
        wallJump =  new WallJump(this,"Jump",stateMachine);
        wallSlide = new WallSlide( this, "WallSlide",stateMachine); 
        wallHangState= new WallHangState(this,"WallHang",stateMachine);
        ledgeClimbState= new LedgeClimbState(this,"LedgeClimb",stateMachine);
        crouchIdle = new CrouchIdle(this,"CrouchIdle",stateMachine);
        crouchMove = new CrouchMove(this,"CrouchMove",stateMachine);
        dashState = new DashState(this,"Dash",stateMachine);
        slideState = new SlideState(this,"Slide",stateMachine);
        doubleJump = new DoubleJump(this,"FlipJump",stateMachine);

        meleeAtt1 = new MeleeAtt1(this,"Attack1",stateMachine);
        meleeAtt2 = new MeleeAtt2(this,"Attack2",stateMachine);
        meleeRun = new MeleeRun(this,"MeleeRun",stateMachine);
        meleeSpin = new MeleeSpin(this,"MeleeSpin",stateMachine);
        kick = new Kick(this,"Kick",stateMachine);
        crouchAttack = new CrouchAttack(this,"CrouchAttack",stateMachine);

        playerDeath= new PlayerDeath(this,"Death",stateMachine);
        getHit= new GetHit(this,"GetHit",stateMachine);

        shieldBlock = new ShieldBlock(this,"Block",stateMachine);
        crouchBlock = new CrouchBlock(this,"CrouchBlock", stateMachine);
    }

    void OnEnable()
    {
        input.Movement.Enable();    
        
        input.Movement.VerticalMove.performed += ctx => inputVector = ctx.ReadValue<Vector2>();
        input.Movement.VerticalMove.canceled += ctx => inputVector = Vector2.zero;
        
         input.Movement.Attack1.performed +=ctx => {if(stateMachine.currentState == getHit) return; if(isGrounded&&!isAttacking)stateMachine.ChangeState(meleeAtt1);};
         input.Movement.Attack2.performed +=ctx => {if(stateMachine.currentState == getHit) return; if(isGrounded&&!isAttacking)stateMachine.ChangeState(meleeAtt2);};
         input.Movement.MeleeRun.performed+= ctx =>{if(stateMachine.currentState == getHit) return; if(isGrounded&&!isAttacking)stateMachine.ChangeState(meleeRun);};
         input.Movement.MeleeSpin.performed+= ctx =>{if(stateMachine.currentState == getHit) return; if(isGrounded&&!isAttacking)stateMachine.ChangeState(meleeSpin);};
         input.Movement.Kick.performed+= ctx =>{if(stateMachine.currentState == getHit) return; if(isGrounded&&!isAttacking)stateMachine.ChangeState(kick);};
        
        input.Movement.Dash.performed+= ctx => 
        {if(stateMachine.currentState == getHit) return; 
            if(cooldownTimer<=0)
            {
            stateMachine.ChangeState(dashState);
            }
        };
        
            input.Movement.CrouchAtt.performed+=ctx=>{
                if(inputVector.y<0)
                {
                    stateMachine.ChangeState(crouchAttack);
                }else{
                }
                
                };    
            input.Movement.CrouchBlock.performed+= ctx=>stateMachine.ChangeState(crouchBlock);
            input.Movement.Block.performed+=ctx=>{
                if(inputVector.y>=0)
                {stateMachine.ChangeState(shieldBlock);}
                };
            input.Movement.Jump.performed+=ctx=>
            {
             if(stateMachine.currentState == getHit) return;
             Checks();
               
                if(isGrounded)
                {
                    stateMachine.ChangeState(jump);
                    return;
                }
                else if(isWallDetected&&isTouchingLedge&&!isClimbingLedge)
                {
                    stateMachine.ChangeState(wallJump);
                    return;
                }

                if(isHandging && !isClimbingLedge)
                {
                    stateMachine.ChangeState(ledgeClimbState);
                    return;
                }

                if(jumpCount<maxJumps)
                {
                    stateMachine.ChangeState(doubleJump);
                }
                   
            };
            
    }

    void OnDisable()
    {
        input.Movement.Disable();
    }
    void Start()
    {
        stateMachine.Initialize(idle);
    }
    
    void Update()
    {   
        inputVector = input.Movement.VerticalMove.ReadValue<Vector2>();
        Checks();

        if (stateMachine.currentState == getHit)
        {
            getHit.Update();
            return;
        }

       if(health<=0)
        {
            stateMachine.ChangeState(playerDeath);
        }

        if(isForcedCrouch && stateMachine.currentState != crouchIdle && stateMachine.currentState != crouchMove)
        {
            stateMachine.ChangeState(crouchIdle);
        }

        cooldownTimer-= Time.deltaTime;

        stateMachine.currentState.Update();

        if(isGrounded||!isWallJumping)
        {
        FlipController();
        }
        anim.SetFloat("YVelocity",rb.linearVelocityY);

        if(!isClimbingLedge && isWallDetected&&isGrounded&&inputVector.x!=0 && !isForcedCrouch && stateMachine.currentState != crouchIdle && stateMachine.currentState != crouchMove)
        {
            stateMachine.ChangeState(idle);
        }

        if (!isClimbingLedge && !isForcedCrouch && stateMachine.currentState!=wallSlide && isWallDetected&&!isTouchingLedge&&inputVector.y>=0)
        {
            stateMachine.ChangeState(wallHangState);
        }

    }

    void FixedUpdate()
    {
        if (stateMachine.currentState == getHit)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            stateMachine.currentState.FixedUpdate();
            return;
        }

        stateMachine.currentState.FixedUpdate();
    }
    public void FlipController()
    {
        //could make animation for rotation by creating a new state with rotating and either time based or event based
        if (isAttacking || stateMachine.currentState == slideState)
        {
            return;
        }
            
            if(inputVector.x > 0.1f&&!isFacingRight)
            {   
                    Flip(inputVector.x);
                
            }else if(inputVector.x < -0.1f&&isFacingRight)
            {
                    Flip(inputVector.x);    
            }
        
    }

    public void Flip(float value)
    {   
        if (isAttacking || stateMachine.currentState == slideState)
        {
            return;
        }

        int direction = value >= 0 ? 1 : -1;
        transform.localScale = new Vector3(direction, 1, 1);
        isFacingRight = direction == 1;
        facDir = direction;
    }

   

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position,groundCheck.position + new Vector3(0,-groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position,wallCheck.position+ new Vector3(facDir*wallCheckDistance,0,0));
        Gizmos.DrawLine(hangCheck.position,hangCheck.position+ new Vector3(facDir*hangCheckDistance,0,0));

       
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(forcedCrouchCheck.position, forcedCrouchCheck.position + new Vector3(0, -forcedCrouchCheckDistance, 0));
        
    }

    void OnDrawGizmosSelected()
    {
        if (meleeAtt1AttackPos1 != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(meleeAtt1AttackPos1.transform.position, attack1Distance1);
        }

        if (meleeAtt2AttackPos2 != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(meleeAtt2AttackPos2.transform.position, attack2Distance);
        }

        if(meleeRunAttackPos3 != null)
        {
            Gizmos.color= Color.blue;
            Gizmos.DrawWireSphere(meleeRunAttackPos3.transform.position,attack3Distance);
        }

         if(meleeSpinAttackPos4 != null)
        {
            Gizmos.color= Color.purple;
            Gizmos.DrawWireSphere(meleeSpinAttackPos4.transform.position,attack4Distance);
        }

         if(meleeAttKickAttackPos5 != null)
        {
            Gizmos.color= Color.green;
            Gizmos.DrawWireSphere(meleeAttKickAttackPos5.transform.position,attack5Distance);
        }
        if(meleeAttCrouchAttackPos6!=null)
        {
            Gizmos.color= Color.purple;
            Gizmos.DrawWireSphere(meleeAttCrouchAttackPos6.transform.position,attack6Distance);
        }
    }

    void Checks()
    {
        isGrounded= Physics2D.Raycast(groundCheck.position,Vector2.down,groundCheckDistance,whatIsGround);
        isWallDetected= Physics2D.Raycast(wallCheck.position,Vector2.right,wallCheckDistance*facDir,whatIsGround);
        isTouchingLedge= Physics2D.Raycast(hangCheck.position,Vector2.right,hangCheckDistance*facDir,whatIsGround);

        if (forcedCrouchCheck != null)
        {
            isForcedCrouchDetected = Physics2D.Raycast(forcedCrouchCheck.position, Vector2.down, forcedCrouchCheckDistance, whatIsGround);
        }
        else
        {
            isForcedCrouchDetected = false;
        }

        isForcedCrouch = isForcedCrouchDetected && isGrounded;
    }

    public void Attack1Checks()
    {
        enemiesInAttackRange.Clear();

        

       
        int dealingDamage = 0;

        if (stateMachine.currentState == kick)
        {
            dealingDamage = 10;
        }
        else if (stateMachine.currentState == meleeAtt1)
        {
            dealingDamage = 15;
        }
        else if (stateMachine.currentState == meleeAtt2)
        {
            dealingDamage = 20;
        }
        else if (stateMachine.currentState == meleeRun)
        {
            dealingDamage = 25;
        }
        else if (stateMachine.currentState == meleeSpin)
        {
            dealingDamage = 30;
        }else if(stateMachine.currentState == crouchAttack)
        {
            dealingDamage= 15;
        }
        else
        {
            Debug.LogWarning("Attack1Checks called while current state is not a valid attack state.");
            return;
        }
        Transform attackOrigin = null;
        float attackRadius = 0f;

        if (stateMachine.currentState == kick)
        {
            attackOrigin = meleeAttKickAttackPos5 != null ? meleeAttKickAttackPos5.transform : null;
            attackRadius = attack5Distance;
        }
        else if (stateMachine.currentState == meleeAtt1)
        {
            attackOrigin = meleeAtt1AttackPos1 != null ? meleeAtt1AttackPos1.transform : null;
            attackRadius = attack1Distance1;
        }
        else if (stateMachine.currentState == meleeAtt2)
        {
            attackOrigin = meleeAtt2AttackPos2 != null ? meleeAtt2AttackPos2.transform : null;
            attackRadius = attack2Distance;
        }
        else if (stateMachine.currentState == meleeRun)
        {
            attackOrigin = meleeRunAttackPos3 != null ? meleeRunAttackPos3.transform : null;
            attackRadius = attack3Distance;
        }
        else if (stateMachine.currentState == meleeSpin)
        {
            attackOrigin = meleeSpinAttackPos4 != null ? meleeSpinAttackPos4.transform : null;
            attackRadius = attack4Distance;
        }
        else if (stateMachine.currentState == crouchAttack)
        {
            attackOrigin = meleeAttCrouchAttackPos6 != null ? meleeAttCrouchAttackPos6.transform : null;
            attackRadius = attack6Distance;
        }

        if (attackOrigin == null)
        {
            Debug.LogWarning("Attack1Checks called with no valid attack origin for the current state.");
            return;
        }

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(
            attackOrigin.position,
            attackRadius,
            whatIsEnemy);

        foreach (Collider2D hitCollider in hitColliders)
        {
            EnemyScript enemy = hitCollider.GetComponentInParent<EnemyScript>();

            if (enemy != null && !enemiesInAttackRange.Contains(enemy))
            {
                enemiesInAttackRange.Add(enemy);
            }
        }

        Debug.Log("I will hit " + enemiesInAttackRange.Count + " enemies with " + dealingDamage + " damage");
        foreach (var enemy in enemiesInAttackRange)
        {
            TakeDamage(enemy, dealingDamage);
        }
    }
    public void AnimationFinishCalled()
    {
       
        finishedCLimb=true;
    }
    public void AniimationClimbCalled()
    {
        moveUp=true;
    }
    public void AnimationMoveForwardCalled()
    {
        moveForward=true;
    }

    public void AttackFinish()
    {
        finishAttack=true;
    }

    public void StartKickForwardMovement()
    {
        kickDamageDealt = false;
        rb.linearVelocity = new Vector2(kickMoveSpeed * facDir, rb.linearVelocity.y);
    }

    public void StartKickBackwardMovement()
    {
        kickDamageDealt = true;
        rb.linearVelocity = new Vector2(-kickMoveSpeed * facDir, rb.linearVelocity.y);
    }

    public void ResetKickMovement()
    {
        kickDamageDealt = false;
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    public void HitByEnemy(Transform attacker = null)
    {
        if (attacker != null)
        {
            // Calculate direction to attacker
            float directionToAttacker = Mathf.Sign(attacker.position.x - transform.position.x);
            // If attacker is at same position, default to current facing
            if (directionToAttacker == 0) directionToAttacker = facDir;
            
            facDir = (int)directionToAttacker;
            // Only flip if not already facing that direction
            if ((facDir > 0 && !isFacingRight) || (facDir < 0 && isFacingRight))
            {
                Flip(facDir);
            }
            
            // Knockback is opposite of direction to attacker
            getHitKnockbackDirection = -directionToAttacker;
        }
        else
        {
            getHitKnockbackDirection = -facDir;
        }
        if(stateMachine.currentState!=shieldBlock){
        getHitTimer = getHitDuration;
        rb.linearVelocity = Vector2.zero;
        stateMachine.ChangeState(getHit);
        }
        else
        {
            return;
        }
    }

    public void TakeDamage(int damage, Transform attacker = null)
    {
        health -= damage;
        if (stateMachine.currentState != getHit)
        {
            HitByEnemy(attacker);
        }
    }

    public void TakeDamage(EnemyScript enemy,int damage)
    {
        if (enemy == null)
        {
            return;
           
        }

        enemy.health -= damage;
        enemy.HitByPlayer();
      
    }

   
}
