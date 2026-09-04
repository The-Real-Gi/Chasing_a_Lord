using UnityEngine;

public class BossMageScript : MonoBehaviour
{

    public Animator anim;
    public Rigidbody2D rb;

    [SerializeField] private float idleDuration = 2f;
    public float IdleDuration => idleDuration;

    public int facDir = 1;
    public bool isFacingRight = true;
    
    public MageStateMachine stateMachine;
    public MageMove mageMove {get; private set;}
    public MageIdle mageIdle {get;private set;}
    public MageDeath mageDeath {get; private set;}
    public MageAttack1 mageAttack1 {get;private set;}
    public MageAttack2 mageAttack2 {get;private set;}

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        stateMachine = new MageStateMachine();
        mageIdle = new MageIdle(this, stateMachine, "Idle");
        stateMachine.Initialize(mageIdle);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.currentState.Update();
    }

    public void Flip()
    {
        facDir *= -1;
        isFacingRight = facDir >= 0;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * facDir;
        transform.localScale = scale;
    }
}
