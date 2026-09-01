using UnityEngine;

public class EnemyRoll : EnemyState
{
    private float rollTimer;

    public EnemyRoll(EnemyScript _enemy, EnemyStateMachine _enemyStateMachine, string _enemyAnim) : base(_enemy, _enemyStateMachine, _enemyAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rollTimer = enemy.rollDuration;
        enemy.rb.linearVelocity = new Vector2(enemy.facDir * enemy.rollSpeed, enemy.rb.linearVelocity.y);
        Debug.Log("Entered roll state");
    }


    public override void Update()
    {
        base.Update();

        rollTimer -= Time.deltaTime;
        enemy.rb.linearVelocity = new Vector2(enemy.facDir * enemy.rollSpeed, enemy.rb.linearVelocity.y);

        if (rollTimer <= 0f)
        {
            enemy.rb.linearVelocity = new Vector2(0f, enemy.rb.linearVelocity.y);
            
            stateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.rb.linearVelocity = new Vector2(0f, enemy.rb.linearVelocity.y);
        enemy.Flip();
    }
}
