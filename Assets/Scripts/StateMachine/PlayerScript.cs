using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public Animator anim;
    [HideInInspector]
    public Rigidbody2D rb;

    public StateMachine stateMachine;
    public IdleState idle{get;private set;}
    public MoveState move {get;private set;}
    public JumpState jump{get;private set;}
    public AirState airState{get;private set;}
    public WallSlide wallSlide{get;private set;}
    public WallJump wallJump {get;private set;}


    public Vector2 inputVector;
    public bool isFacingRight=true;
    public int facDir=1;

    public float moveSpeed;
    public float jumpStrength;
    public float wallJumpStrength;

    public Transform groundCheck;
    public float groundCheckDistance;
    public LayerMask whatIsGround;

    public Transform wallCheck;
    public float wallCheckDistance;

    public bool isGrounded;
    public bool isWallDetected;
    public bool isWallJumping=false;


    PlayersInputSet input;

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
    }

    void OnEnable()
    {
        input.Movement.Enable();
        input.Movement.VerticalMove.performed += ctx=> inputVector=ctx.ReadValue<Vector2>();
        input.Movement.VerticalMove.canceled += ctx => inputVector = Vector2.zero;
        
           input.Movement.Jump.performed+=ctx=>
            {
                if(isGrounded)
                {
                    Debug.Log("Performed Jump");
                    stateMachine.ChangeState(jump);
                }else if(isWallDetected)
                {
                    stateMachine.ChangeState(wallJump);
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
        stateMachine.currentState.Update();
        if(isGrounded||!isWallJumping)
        {
        FlipController();
        }
        Checks();
        anim.SetFloat("YVelocity",rb.linearVelocityY);

        if(isWallDetected&&isGrounded&&inputVector.x!=0)
        {
            stateMachine.ChangeState(idle);
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
        if(isWallJumping)
        {
        transform.localScale= new Vector3(-value,1,1);
        }
        else
        {
        transform.localScale= new Vector3(inputVector.x,1,1);           
        }
        isFacingRight=!isFacingRight;
        facDir=facDir*(-1);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position,groundCheck.position + new Vector3(0,-groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position,wallCheck.position+ new Vector3(facDir*wallCheckDistance,0,0));
    }
    void Checks()
    {
        isGrounded= Physics2D.Raycast(groundCheck.position,Vector2.down,groundCheckDistance,whatIsGround);
        isWallDetected= Physics2D.Raycast(wallCheck.position,Vector2.right,wallCheckDistance*facDir,whatIsGround);
    }
}
