using UnityEngine;

public class ArcherShoot : ArcherState
{
    public ArcherShoot(ArcherScript _archerScript, ArcherStateMachine _stateMachine, string _animBoolName) : base(_archerScript, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        archer.FacePlayer();
    }

    public override void Update()
    {
        base.Update();

        if (!archer.IsPlayerInRange())
        {
            archer.shooting = false;
            stateMachine.ChangeState(archer.idle);
            return;
        }

        if (archer.shooting)
        {
            archer.ShootArrow();

            bool canRetreat = archer.IsPlayerTooClose() && archer.isGrounded;
            stateMachine.ChangeState(canRetreat ? archer.move : archer.battleState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
        archer.shooting=false;
    }
}
