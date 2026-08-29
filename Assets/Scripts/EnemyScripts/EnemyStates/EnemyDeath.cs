using System.Threading;
using UnityEngine;

public class EnemyDeath : EnemyState
{
    public EnemyDeath(EnemyScript _enemy, EnemyStateMachine _enemyStateMachine, string _enemyAnim) : base(_enemy, _enemyStateMachine, _enemyAnim)
    {
    }

   public override void Enter()
    {
        base.Enter();
        enemy.rb.linearVelocityX=0;
        enemy.timer=2;
    }

    public override void Update()
    {
        base.Update();
        enemy.timer-=Time.deltaTime;
        if(enemy.timer<=0)
        {
            enemy.gameObject.SetActive(false);
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
