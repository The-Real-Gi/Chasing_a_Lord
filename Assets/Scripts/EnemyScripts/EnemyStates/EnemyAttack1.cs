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
        if(enemy.attackFinish)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
        //check for player in range to check for dealing damage
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
