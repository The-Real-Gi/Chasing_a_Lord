using UnityEngine;

public class EnemyCrouchMove : EnemyState
{
    public EnemyCrouchMove(EnemyScript _enemy, EnemyStateMachine _enemyStateMachine, string _enemyAnim) : base(_enemy, _enemyStateMachine, _enemyAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.ApplyCrouchCollider();
        enemy.timer = enemy.CrouchSearchDuration;
    }
    public override void Update()
    {
        base.Update();

        if (!enemy.MustCrouch())
        {
            stateMachine.ChangeState(enemy.battleState);
            return;
        }

        if (enemy.IsPlayerInCrouchAttackRange())
        {
            if (enemy.CanAttackAgain())
            {
                enemy.FacePlayer();
                stateMachine.ChangeState(enemy.enemyCrouchAttack);
            }
            else
            {
                stateMachine.ChangeState(enemy.enemyCrouchIdle);
            }
            return;
        }

        if (enemy.CanSeePlayer())
        {
            enemy.timer = enemy.CrouchSearchDuration;
            return;
        }

        enemy.timer -= Time.deltaTime;
        if (enemy.timer <= 0f)
        {
            stateMachine.ChangeState(enemy.enemyCrouchIdle);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (enemy.IsPlayerInCrouchAttackRange())
        {
            enemy.rb.linearVelocityX = 0f;
            return;
        }

        enemy.rb.linearVelocityX = enemy.facDir * enemy.moveSpeed * Time.fixedDeltaTime * 0.5f;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.ResetColliderToBase();
    }
}
