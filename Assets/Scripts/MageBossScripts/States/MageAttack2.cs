using System.Collections;
using UnityEngine;

public class MageAttack2 : MageState
{
    private const int BombCount = 4;
    private const float SpawnDelay = 0.1f;
    private const float BombSpacing = 1f;
    private Coroutine bombSpawnRoutine;

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
                Debug.Log("NothingSpawning");
            }
            else if (attnum == 3)
            {
                 Debug.Log("NothingSpawning");
            }
        }

        if(mageScript.attackEnded)
        {
            stateMachine.ChangeState(mageScript.mageBattleState);
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
    }

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
}
