using UnityEngine;

public class MageAttack1 : MageState
{
    public MageAttack1(BossMageScript _bossMageScript, MageStateMachine _stateMachine, string _animBoolName) : base(_bossMageScript, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if(mageScript.spawnObj)
        {
            Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
            FireBall fireBallPrefab = mageScript.fireball?.GetComponent<FireBall>();

            if (player != null && fireBallPrefab != null)
            {
                GameObject fireBall = Object.Instantiate(mageScript.fireball, mageScript.transform.position, Quaternion.identity);
                Vector2 moveDirection = player.position - mageScript.transform.position;
                fireBall.GetComponent<FireBall>().SetUp(moveDirection);
            }

            mageScript.spawnObj = false;
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
        
    }
}
