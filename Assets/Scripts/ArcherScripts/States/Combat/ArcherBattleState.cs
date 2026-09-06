using UnityEngine;

public class ArcherBattleState : ArcherState
{
    private const float tooCloseShootDelay = 1f;
    private float tooCloseTimer;

    public ArcherBattleState(ArcherScript _archerScript, ArcherStateMachine _stateMachine, string _animBoolName) : base(_archerScript, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        tooCloseTimer = 0f;
    }

    public override void Update()
    {
        base.Update();

        if (!archer.IsPlayerInRange())
        {
            stateMachine.ChangeState(archer.idle);
            return;
        }

        archer.FacePlayer();

        if (archer.IsPlayerTooClose())
        {
            if (!archer.isGrounded && archer.IsPlayerInShootingRange())
            {
                stateMachine.ChangeState(archer.shoot);
                return;
            }

            tooCloseTimer += Time.deltaTime;
            if (tooCloseTimer >= tooCloseShootDelay && archer.IsPlayerInShootingRange())
            {
                stateMachine.ChangeState(archer.shoot);
            }

            return;
        }

        tooCloseTimer = 0f;
        if (archer.IsPlayerInShootingRange())
        {
            stateMachine.ChangeState(archer.shoot);
        }
        else
        {
            stateMachine.ChangeState(archer.move);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
