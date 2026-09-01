using UnityEngine;

public class EnemyCrouchIdle : EnemyState
{
    public EnemyCrouchIdle(EnemyScript _enemy, EnemyStateMachine _enemyStateMachine, string _enemyAnim) : base(_enemy, _enemyStateMachine, _enemyAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.ApplyCrouchCollider();
        enemy.rb.linearVelocityX = 0f;
        enemy.timer = enemy.CrouchIdleMoveDelay;
        enemy.Flip();
    }
    public override void Update()
    {
        base.Update();

        if (!enemy.MustCrouch())
        {
            stateMachine.ChangeState(enemy.battleState);
            return;
        }

        if (enemy.CanSeePlayer())
        {
            if (enemy.IsPlayerInAttackRange() && enemy.CanAttackAgain())
            {
                stateMachine.ChangeState(enemy.enemyCrouchAttack);
                return;
            }
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
