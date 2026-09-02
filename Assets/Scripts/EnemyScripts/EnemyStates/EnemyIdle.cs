using UnityEngine;

public class EnemyIdle : EnemyState
{
    public EnemyIdle(EnemyScript _enemy, EnemyStateMachine _enemyStateMachine, string _enemyAnim) : base(_enemy, _enemyStateMachine, _enemyAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.timer=enemy.timeToRun;
    }

    public override void Update()
    {
        base.Update();

        if (enemy.MustCrouch())
        {
            stateMachine.ChangeState(enemy.enemyCrouchIdle);
            return;
        }

        enemy.timer-=Time.deltaTime;
        if(enemy.timer<=0)
        {
            stateMachine.ChangeState(enemy.enemyMove);
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
