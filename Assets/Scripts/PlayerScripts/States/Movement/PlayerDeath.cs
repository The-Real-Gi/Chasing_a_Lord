using UnityEngine;

public class PlayerDeath : PlayerState
{
    public PlayerDeath(PlayerScript _player, string _animName, StateMachine _statemachine) : base(_player, _animName, _statemachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        GameManager.Instance.DeathPanel();
        player.rb.linearVelocity= new Vector2(0,player.rb.linearVelocityY);
        player.playerCollider.offset= new Vector2(0.02f,-0.05f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

    }

    public override void Update()
    {
        base.Update();
    }

}
