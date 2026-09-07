using UnityEngine;

public class MageAttack2 : MageState
{
<<<<<<< Updated upstream
=======
    private const int BombCount = 4;
    private const int ExplosionCount = 3;
    private const float SpawnDelay = 0.1f;
    private const float ExplosionSpawnDelay = 0.2f;
    private const float BombSpacing = 1f;
    private Coroutine bombSpawnRoutine;
    private Coroutine explosionSpawnRoutine;

>>>>>>> Stashed changes
    public MageAttack2(BossMageScript _bossMageScript, MageStateMachine _stateMachine, string _animBoolName) : base(_bossMageScript, _stateMachine, _animBoolName)
    {
    }

     public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
<<<<<<< Updated upstream
        if(mageScript.attackEnded)
=======

        if (mageScript.spawnObj)
        {
            mageScript.spawnObj = false;

            int attnum = Random.Range(1, 4);

            if (attnum == 1)
            {
                bombSpawnRoutine = mageScript.StartCoroutine(SpawnBombs());
                Debug.Log("Spawning bombs");
            }
            else if (attnum == 2)
            {
                explosionSpawnRoutine = mageScript.StartCoroutine(SpawnExplosions());
            }
            else if (attnum == 3)
            {
                if (mageScript.SpawningMeleeEnemy != null && mageScript.meleeEnemySpawnPoint != null)
                {
                    Object.Instantiate(mageScript.SpawningMeleeEnemy, mageScript.meleeEnemySpawnPoint.position, Quaternion.identity);
                }
            }
        }

        if (mageScript.attackEnded && bombSpawnRoutine == null && explosionSpawnRoutine == null)
>>>>>>> Stashed changes
        {
            stateMachine.ChangeState(mageScript.mageBattleState);
        }

        if(mageScript.spawnObj)
        {
            int attnum=Random.Range(1,3);

            if(attnum==1)
            {
                
            }else if (attnum==2)
            {
                
            }else if (attnum==3)
            {
                
            }
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
        mageScript.attackEnded=false;
        mageScript.spawnObj = false;

        if (bombSpawnRoutine != null)
        {
            mageScript.StopCoroutine(bombSpawnRoutine);
            bombSpawnRoutine = null;
        }

        if (explosionSpawnRoutine != null)
        {
            mageScript.StopCoroutine(explosionSpawnRoutine);
            explosionSpawnRoutine = null;
        }
    }
<<<<<<< Updated upstream
=======

    private IEnumerator SpawnBombs()
    {
        if (mageScript.droppingBomb == null || mageScript.dropBombSpawnPos == null)
        {
            bombSpawnRoutine = null;
            yield break;
        }

        Transform spawnPoint = mageScript.dropBombSpawnPos.transform;

        for (int bombIndex = 0; bombIndex < BombCount; bombIndex++)
        {
            Vector3 spawnPosition = spawnPoint.position + Vector3.right * (mageScript.facDir * BombSpacing * bombIndex);
            Object.Instantiate(mageScript.droppingBomb, spawnPosition, Quaternion.identity);

            if (bombIndex < BombCount - 1)
            {
                yield return new WaitForSeconds(SpawnDelay);
            }
        }

        bombSpawnRoutine = null;
    }

    private IEnumerator SpawnExplosions()
    {
        if (mageScript.explosion == null)
        {
            explosionSpawnRoutine = null;
            yield break;
        }

        for (int explosionIndex = 0; explosionIndex < ExplosionCount; explosionIndex++)
        {
            Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player == null)
            {
                break;
            }

            Object.Instantiate(mageScript.explosion, player.position, Quaternion.identity);

            if (explosionIndex < ExplosionCount - 1)
            {
                yield return new WaitForSeconds(ExplosionSpawnDelay);
            }
        }

        explosionSpawnRoutine = null;
    }
>>>>>>> Stashed changes
}
