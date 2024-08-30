using UnityEngine;

public class Idol : IStateMachine, ICombo
{
    private Player _player;
    private float _timer;
    private int _stateIndex;

    public Idol(Player player)
    {
        _player = player;
    }

    public void Attack()
    {

    }

    public void Enter()
    {
        _player.Anim.SetInteger("AttackIndex", 0);
        _timer = 0;
    }

    public void Exit()
    {
        _player.NextAttack((Player.AttackState)_stateIndex);
    }

    public void FixedUpdate()
    {
    }

    public void Update()
    {
        //_timer += Time.deltaTime;
        if(_timer > _player.ComboInterval)
        {
            _stateIndex = (int)Player.AttackState.Idol;
            Exit();
        }

        if(Input.GetButtonDown("Fire1"))
        {
            _stateIndex = (int)Player.AttackState.Attack1;
            Exit();
        }
    }
}
