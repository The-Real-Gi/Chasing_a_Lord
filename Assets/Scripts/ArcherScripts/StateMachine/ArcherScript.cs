using UnityEngine;

public class ArcherScript : MonoBehaviour
{

    public Animator anim;
    ArcherStateMachine stateMachine;
    public ArcherIdle idle {get;private set;}
    public ArcherMove move {get;private set;}
    public ArcherShoot shoot {get;private set;}
    public ArcherGetHit getHit {get;private set;}
    public ArcherDeath death {get;private set;}

    void Awake()
    {
        stateMachine = new ArcherStateMachine();
        idle = new ArcherIdle (this,stateMachine,"Idle");
        move = new ArcherMove (this,stateMachine,"Move");
        shoot = new ArcherShoot(this, stateMachine,"Shoot");
        getHit = new ArcherGetHit (this,stateMachine,"GetHit");
        death = new ArcherDeath(this,stateMachine,"Die");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
