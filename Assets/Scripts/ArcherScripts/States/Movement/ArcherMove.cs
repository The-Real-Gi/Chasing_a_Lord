using UnityEngine;

public class ArcherMove : ArcherState
{
    public ArcherMove(ArcherScript _archerScript, ArcherStateMachine _stateMachine, string _animBoolName) : base(_archerScript, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (archer.IsPlayerTooClose())
        {
            archer.FaceAwayFromPlayer();
        }
        else
        {
            archer.FacePlayer();
        }
    }

    public override void Update()
    {
        base.Update();

        if (!archer.IsPlayerInRange())
        {
            stateMachine.ChangeState(archer.idle);
            return;
        }

        if (!archer.isGrounded && archer.IsPlayerTooClose() && archer.IsPlayerInShootingRange())
        {
            stateMachine.ChangeState(archer.shoot);
            return;
        }

        if (!archer.IsPlayerTooClose() && archer.IsPlayerInShootingRange())
        {
            stateMachine.ChangeState(archer.battleState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!archer.isGrounded && archer.IsPlayerTooClose())
        {
            if (archer.rb != null)
            {
                archer.rb.linearVelocityX = 0f;
            }

            return;
        }

        if (archer.IsPlayerTooClose())
        {
            archer.FaceAwayFromPlayer();
        }
        else
        {
            archer.FacePlayer();
        }

        if (archer.rb != null)
        {
            Vector2 retreatStep = Vector2.right * archer.facDir * archer.moveSpeed * Time.fixedDeltaTime;
            archer.rb.MovePosition(archer.rb.position + retreatStep);
        }
    }

    public override void Exit()
    {
        base.Exit();
        archer.rb.linearVelocityX = 0f;
    }
}
