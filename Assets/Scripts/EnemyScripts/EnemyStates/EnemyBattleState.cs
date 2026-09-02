using UnityEngine;

public class EnemyBattleState : EnemyState
{
    private const float loseSightTimer = 2f;

    public EnemyBattleState(EnemyScript _enemy, EnemyStateMachine _enemyStateMachine, string _enemyAnim) : base(_enemy, _enemyStateMachine, _enemyAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.timer = loseSightTimer;
    }

    public override void Update()
    {
        base.Update();

        // A low ceiling (or the second ceiling check) keeps the enemy in its
        // crouched pursuit mode, regardless of which player ray sees them.
        if (enemy.MustCrouch())
        {
            stateMachine.ChangeState(enemy.enemyCrouchMove);
            return;
        }

        if (enemy.IsPlayerInAttackRange() && enemy.CanAttackAgain())
        {
            enemy.FacePlayer();
            int randomNumber = Random.Range(1, 3);
            Debug.Log(randomNumber);

            if (randomNumber == 1)
            {
                stateMachine.ChangeState(enemy.enemyAttack1);
                return;
            }
            else if (randomNumber == 2)
            {
                stateMachine.ChangeState(enemy.enemyAttack2);
                return;
            }
            else
            {
                Debug.Log("error in choosing the number");
            }
        }

        if (enemy.IsPlayerInAttackRange() && !enemy.CanAttackAgain())
        {
            enemy.FacePlayer();
            enemy.timer = loseSightTimer;
            if (enemy.MustCrouch())
            {
                stateMachine.ChangeState(enemy.enemyCrouchIdle);
            }
            else
            {
                enemy.anim.SetBool("Move", false);
                enemy.anim.SetBool("Idle", true);
                enemy.anim.SetBool("Attack1", false);
            }
            return;
        }

        enemy.anim.SetBool("Idle", false);
        enemy.anim.SetBool("Move", true);
        enemy.anim.SetBool("Attack1", false);

        if (enemy.isSeeingPlayer)
        {
            enemy.timer = loseSightTimer;
            return;
        }

        enemy.timer -= Time.deltaTime;

        if (enemy.timer <= 0f)
        {
            stateMachine.ChangeState(enemy.enemyIdle);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (enemy.IsPlayerInAttackRange() && !enemy.CanAttackAgain())
        {
            enemy.rb.linearVelocityX = 0f;
            enemy.anim.speed = 1f;
            return;
        }

        enemy.anim.SetBool("Idle", false);
        enemy.anim.SetBool("Move", true);
        enemy.anim.SetBool("Attack1", false);
        enemy.rb.linearVelocityX = enemy.facDir * enemy.moveSpeed * Time.fixedDeltaTime * 2;
        enemy.anim.speed = 2;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.anim.speed = 1;
    }
}
