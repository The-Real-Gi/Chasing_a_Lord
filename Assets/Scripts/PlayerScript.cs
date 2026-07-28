using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public Animator anim;
    [HideInInspector]
    public Rigidbody2D rb;

    public StateMachine stateMachine;
    public IdleState idle{get;private set;}
    public MoveState move {get;private set;}
    public Vector2 inputVector;

    public float moveSpeed;


    PlayersInputSet input;

    void Awake()
    {   input = new PlayersInputSet();
        anim = GetComponentInChildren<Animator>();
        stateMachine= new StateMachine();
        rb= GetComponent<Rigidbody2D>();
        idle= new IdleState(this,"Idle",stateMachine);
        move= new MoveState(this,"Run",stateMachine);
    }

    void OnEnable()
    {
        input.Movement.Enable();
        input.Movement.VerticalMove.performed += ctx=> inputVector=ctx.ReadValue<Vector2>();
        input.Movement.VerticalMove.canceled += ctx => inputVector = Vector2.zero;
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
    }

    void FixedUpdate()
    {
        stateMachine.currentState.FixedUpdate();
    }
}
