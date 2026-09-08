using System.Collections;
using UnityEngine;

public class MageAttack1 : MageState
{
    private const float SpawnDelay = 0.1f;
    private const float BallHeightOffset = 0.35f;
    private const float UpwardLaunchForce = 1.5f;
    private const float UpwardAimIncrement = -0.25f;
    private const float HorizontalAimMultiplier = 0.15f;
    private Coroutine multiPurpleBallRoutine;
    private bool useMultiPurpleBalls;

    public MageAttack1(BossMageScript _bossMageScript, MageStateMachine _stateMachine, string _animBoolName) : base(_bossMageScript, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        useMultiPurpleBalls = Random.Range(0, 2) == 1;
    }

    public override void Update()
    {
        base.Update();

        if (mageScript.spawnObj)
        {
            mageScript.spawnObj = false;

            if (useMultiPurpleBalls)
            {
                multiPurpleBallRoutine = mageScript.StartCoroutine(SpawnMultiPurpleBalls());
            }
            else
            {
                SpawnFireBall();
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
        mageScript.StartAttackCooldown();

        mageScript.attackEnded=false;
        mageScript.spawnObj = false;
    }

    private IEnumerator SpawnMultiPurpleBalls()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        MultiPurpleBall multiPurpleBallPrefab = mageScript.multiPurpleBall?.GetComponent<MultiPurpleBall>();
        Transform spawnPoint = mageScript.multiPurpleBallSpawnPoint != null
            ? mageScript.multiPurpleBallSpawnPoint
            : mageScript.transform;

        if (player == null || multiPurpleBallPrefab == null)
        {
            yield break;
        }

        Vector3 targetPosition = player.position;

        for (int ballIndex = 0; ballIndex < 3; ballIndex++)
        {
            Vector3 spawnPosition = spawnPoint.position + Vector3.up * (BallHeightOffset * ballIndex);
            float ballUpwardAimOffset = UpwardLaunchForce + UpwardAimIncrement * ballIndex;
            Vector2 moveDirection = new Vector2(
                ((targetPosition.x - spawnPosition.x) * HorizontalAimMultiplier)/5,
                ballUpwardAimOffset/2);
            GameObject multiPurpleBall = Object.Instantiate(mageScript.multiPurpleBall, spawnPosition, Quaternion.identity);
            multiPurpleBall.GetComponent<MultiPurpleBall>().SetUp(moveDirection);

            if (ballIndex < 2)
            {
                yield return new WaitForSeconds(SpawnDelay);
            }
        }

        multiPurpleBallRoutine = null;
    }

    private void SpawnFireBall()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        FireBall fireBallPrefab = mageScript.fireball?.GetComponent<FireBall>();

        if (player == null || fireBallPrefab == null)
        {
            return;
        }

        GameObject fireBall = Object.Instantiate(mageScript.fireball, mageScript.transform.position, Quaternion.identity);
        Vector2 moveDirection = player.position - mageScript.transform.position;
        fireBall.GetComponent<FireBall>().SetUp(moveDirection);
    }
}
