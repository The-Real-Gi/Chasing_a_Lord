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
    public Vector2 inputVector;
    public bool isFacingRight=true;

    public float moveSpeed;
    public float jumpStrength;

    public Transform groundCheck;
    public float groundCheckDistance;
    public LayerMask whatIsGround;

    public bool isGrounded;


    PlayersInputSet input;

    void Awake()
    {   input = new PlayersInputSet();
        anim = GetComponentInChildren<Animator>();
        stateMachine= new StateMachine();
        rb= GetComponent<Rigidbody2D>();
        idle= new IdleState(this,"Idle",stateMachine);
        move= new MoveState(this,"Run",stateMachine);
        jump= new JumpState(this, "Jump",stateMachine);
        airState= new AirState(this,"Jump",stateMachine);
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
        FlipController();
        Checks();
        anim.SetFloat("YVelocity",rb.linearVelocityY);
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
            Flip();
        }else if(inputVector.x==-1&&isFacingRight)
        {
            Flip();
        }
    }

    public void Flip()
    {
        transform.localScale= new Vector3(inputVector.x,1,1);
        isFacingRight=!isFacingRight;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position,groundCheck.position + new Vector3(0,-groundCheckDistance));
    }
    void Checks()
    {
        isGrounded= Physics2D.Raycast(groundCheck.position,Vector2.down,groundCheckDistance,whatIsGround);
    }
}
