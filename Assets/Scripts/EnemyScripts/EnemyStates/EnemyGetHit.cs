using UnityEngine;

public class EnemyGetHit : EnemyState
{
    public EnemyGetHit(EnemyScript _enemy, EnemyStateMachine _enemyStateMachine, string _enemyAnim) : base(_enemy, _enemyStateMachine, _enemyAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        enemy.getHitFinish = false;
    }
    public override void Update()
    {
        base.Update();
        enemy.UpdateGetHitState();
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
