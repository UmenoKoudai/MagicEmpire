using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack3 : IStateMachine, ICombo
{
    private Player _player;
    private float _timer;
    private int _stateIndex;

    public Attack3(Player player)
    {
        _player = player;
    }

    public void Attack()
    {
        _stateIndex = (int)Player.AttackState.Attack2;
        Exit();
    }

    public void Enter()
    {
        _player.Anim.SetInteger("AttackIndex", (int)Player.AttackState.Attack1);
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
        _timer += Time.deltaTime;
        if(Input.GetButtonDown("Fire1"))
        {
            _stateIndex = (int)Player.AttackState.Idol;
            Exit();
        }
        if(_timer > _player.ComboInterval)
        {
            _stateIndex = (int)Player.AttackState.Idol;
            Exit();
        }
    }
}
