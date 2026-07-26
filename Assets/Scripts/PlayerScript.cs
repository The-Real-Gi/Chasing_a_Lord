using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public Animator anim;
    public Rigidbody2D rb{get;private set;}

    public StateMachine stateMachine;
    public IdleState idle{get;private set;}
    public Vector2 inputVector;


    PlayersInputSet input;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        stateMachine= new StateMachine();
        idle= new IdleState(this,"Idle",stateMachine);
        input = new PlayersInputSet();
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
