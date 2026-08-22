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
    #endregion

    public Vector2 inputVector;

    #region Flip
    public bool isFacingRight=true;
    public int facDir=1;
    #endregion

    #region Stats
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
    public bool isGrounded;
    public bool isWallDetected;
    public bool isWallJumping=false;
    public bool isHandging=false;
    public bool isTouchingLedge=false;
    public bool ledgeDetected;
    public bool canClimbLedge=false;
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
    
    public PlayersInputSet input;

    void Awake()
    {   input = new PlayersInputSet();
        anim = GetComponentInChildren<Animator>();
        stateMachine = new StateMachine();
        rb = GetComponent<Rigidbody2D>();
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
    }

    void OnEnable()
    {
        input.Movement.Enable();    
        input.Movement.VerticalMove.started += ctx => inputVector = ctx.ReadValue<Vector2>();
        input.Movement.VerticalMove.performed += ctx => inputVector = ctx.ReadValue<Vector2>();
        input.Movement.VerticalMove.canceled += ctx => inputVector = Vector2.zero;
        
           input.Movement.Jump.performed+=ctx=>
            {
               
                 if(isWallDetected&&isTouchingLedge&&!isClimbingLedge)
                {
                    stateMachine.ChangeState(wallJump);
                }
               
                if(isGrounded)
                {
                    Debug.Log("Performed Jump");
                    stateMachine.ChangeState(jump);
                }
                if(isHandging && !isClimbingLedge)
                {
                stateMachine.ChangeState(ledgeClimbState);
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
        stateMachine.currentState.Update();
        if(isGrounded||!isWallJumping)
        {
        FlipController();
        }
        Checks();
        anim.SetFloat("YVelocity",rb.linearVelocityY);

        if(!isClimbingLedge && isWallDetected&&isGrounded&&inputVector.x!=0)
        {
            stateMachine.ChangeState(idle);
        }

        if (!isClimbingLedge && isWallDetected&&!isTouchingLedge&&inputVector.y>=0)
        {
            stateMachine.ChangeState(wallHangState);
        }
     

    }

    void FixedUpdate()
    {
        stateMachine.currentState.FixedUpdate();
    }
    public void FlipController()
    {
        //could make animation for rotation by creating a new state with rotating and either time based or event based
            if(inputVector.x==1&&!isFacingRight)
            {
                Flip(inputVector.x);
            }else if(inputVector.x==-1&&isFacingRight)
            {
                Flip(inputVector.x);
            }
        
    }

    public void Flip(float value)
    {   
        transform.localScale= new Vector3(value,1,1);
        
        isFacingRight=!isFacingRight;
        facDir=facDir*(-1);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position,groundCheck.position + new Vector3(0,-groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position,wallCheck.position+ new Vector3(facDir*wallCheckDistance,0,0));
        Gizmos.DrawLine(hangCheck.position,hangCheck.position+ new Vector3(facDir*hangCheckDistance,0,0));

    }
    void Checks()
    {
        isGrounded= Physics2D.Raycast(groundCheck.position,Vector2.down,groundCheckDistance,whatIsGround);
        isWallDetected= Physics2D.Raycast(wallCheck.position,Vector2.right,wallCheckDistance*facDir,whatIsGround);
        isTouchingLedge= Physics2D.Raycast(hangCheck.position,Vector2.right,hangCheckDistance*facDir,whatIsGround);

       
    }
    public void AnimationFinishCalled()
    {
        Debug.Log("Finished animation");
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
}
