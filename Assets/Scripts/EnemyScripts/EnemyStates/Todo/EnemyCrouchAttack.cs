using UnityEngine;

public class EnemyCrouchAttack : EnemyState
{
    public EnemyCrouchAttack(EnemyScript _enemy, EnemyStateMachine _enemyStateMachine, string _enemyAnim) : base(_enemy, _enemyStateMachine, _enemyAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.BeginAttack();
        enemy.rb.linearVelocity = Vector2.zero;
    }

    public override void Update()
    {
        base.Update();

        if (!enemy.attackFinish)
        {
            return;
        }

        stateMachine.ChangeState(enemy.MustCrouch() ? enemy.enemyCrouchMove : enemy.battleState);
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
