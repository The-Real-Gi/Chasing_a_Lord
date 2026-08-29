using UnityEngine;

public class EnemyAttack1 : EnemyState
{
    public EnemyAttack1(EnemyScript _enemy, EnemyStateMachine _enemyStateMachine, string _enemyAnim) : base(_enemy, _enemyStateMachine, _enemyAnim)
    {
    }  
    
    public override void Enter()
    {
        base.Enter();
        enemy.rb.linearVelocity=  Vector2.zero;
    }

    public override void Update()
    {
        base.Update();
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
