using UnityEngine;

public class EnemyState
{
    protected EnemyScript enemy;
    protected string enemyAnim;
    protected EnemyStateMachine stateMachine;

    public EnemyState(EnemyScript _enemy, EnemyStateMachine _enemyStateMachine,string _enemyAnim)
    {
        this.enemy=_enemy;
        this.enemyAnim=_enemyAnim;
        this.stateMachine=_enemyStateMachine;
    }

    public virtual void Enter()
    {
        ClearOtherAnimationBools();
        enemy.anim.SetBool(enemyAnim,true);
    }

    public virtual void Update()
    {
        
    }

    public virtual void FixedUpdate()
    {
        
    }

    public virtual void Exit()
    {
        enemy.anim.SetBool(enemyAnim,false);
    }

    protected void ClearOtherAnimationBools()
    {
        enemy.anim.SetBool("Idle", false);
        enemy.anim.SetBool("Move", false);
        enemy.anim.SetBool("Attack1", false);
        enemy.anim.SetBool("death", false);
    }
}
