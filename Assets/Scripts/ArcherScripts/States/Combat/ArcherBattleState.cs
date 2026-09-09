using UnityEngine;

public class ArcherBattleState : ArcherState
{
    private const float tooCloseShootDelay = 5f;
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
        archer.anim.SetBool("Move", true);
        archer.anim.SetBool("Idle", false);

        if (!archer.IsPlayerInRange())
        {
            stateMachine.ChangeState(archer.idle);
            return;
        }

        if (archer.IsPlayerTooClose())
        {
            tooCloseTimer += Time.deltaTime;
            if (tooCloseTimer >= tooCloseShootDelay
                && archer.CanShoot
                && archer.IsPlayerInShootingRange())
            {
                stateMachine.ChangeState(archer.shoot);
            }

            return;
        }

        tooCloseTimer = 0f;
        if (!archer.IsPlayerInShootingRange())
        {
            stateMachine.ChangeState(archer.move);
            return;
        }

        if (!archer.CanShoot && !archer.IsAtShootingRangeLimit())
        {
            return;
        }

        if (!archer.CanShoot)
        {
            archer.anim.SetBool("Move", false);
            archer.anim.SetBool("Idle", true);
            return;
        }

        archer.FacePlayer();
        if (archer.CanShoot)
        {
            stateMachine.ChangeState(archer.shoot);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        bool shouldMoveAway = archer.IsPlayerTooClose()
            || (!archer.CanShoot
                && archer.IsPlayerInShootingRange()
                && !archer.IsAtShootingRangeLimit());

        if (shouldMoveAway && archer.rb != null)
        {
            archer.FaceAwayFromPlayer();
            Vector2 retreatStep = Vector2.right * archer.facDir * archer.moveSpeed * Time.fixedDeltaTime;
            archer.rb.MovePosition(archer.rb.position + retreatStep);
        }
    }

    public void ResetTooCloseShootTimer()
    {
        tooCloseTimer = 0f;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
