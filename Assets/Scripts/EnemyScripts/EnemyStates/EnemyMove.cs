using UnityEngine;

public class EnemyMove : EnemyState
{
    public EnemyMove(EnemyScript _enemy, EnemyStateMachine _enemyStateMachine, string _enemyAnim) : base(_enemy, _enemyStateMachine, _enemyAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if(enemy.isWallDetected)
        {
            stateMachine.ChangeState(enemy.enemyIdle);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        enemy.rb.linearVelocityX= enemy.facDir*enemy.moveSpeed*Time.fixedDeltaTime;

    }

    public override void Exit()
    {
        base.Exit();
    }
}
