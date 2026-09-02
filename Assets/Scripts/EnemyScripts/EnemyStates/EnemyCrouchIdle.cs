using UnityEngine;

public class EnemyCrouchIdle : EnemyState
{
    private bool flipOnEnter;

    public EnemyCrouchIdle(EnemyScript _enemy, EnemyStateMachine _enemyStateMachine, string _enemyAnim) : base(_enemy, _enemyStateMachine, _enemyAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.ApplyCrouchCollider();
        enemy.rb.linearVelocityX = 0f;
        enemy.timer = enemy.CrouchIdleMoveDelay;
        if (flipOnEnter)
        {
            enemy.Flip();
            flipOnEnter = false;
        }
    }

    public void PrepareForPatrolEntry()
    {
        flipOnEnter = true;
    }
    public override void Update()
    {
        base.Update();

        if (!enemy.MustCrouch())
        {
            stateMachine.ChangeState(enemy.battleState);
            return;
        }

        bool playerInRange = enemy.IsPlayerInAttackRange();

        if (playerInRange && enemy.CanAttackAgain())
        {
            enemy.FacePlayer();
            stateMachine.ChangeState(enemy.MustCrouch() ? enemy.enemyCrouchAttack : enemy.battleState);
            return;
        }

        if (!playerInRange && enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(enemy.enemyCrouchMove);
            return;
        }

        enemy.timer -= Time.deltaTime;
        if (enemy.timer <= 0f)
        {
            stateMachine.ChangeState(enemy.enemyCrouchMove);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.ResetColliderToBase();
    }
}
