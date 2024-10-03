using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack3 : IStateMachine, ICombo
{
    private Player _player;
    private float _timer;
    private int _stateIndex;

    public Attack3(Player player)
    {
        _player = player;
    }

    public void StrongAttack()
    {
        _stateIndex = (int)Player.AttackState.StrongAttack3;
        Exit();
    }

    public void WeakAttack()
    {
        _stateIndex = (int)Player.AttackState.Attack1;
        Exit();
    }


    public async void Enter()
    {
        _timer = 0;
        _player.Anim.SetTrigger("WeakAttack");
        _player.StateChange(Player.MoveState.Stop);
        _player.Rb.velocity = Vector3.zero;
        if (_player.InRangeEnemy.Count <= 0) return;
        foreach (var enemy in _player.InRangeEnemy)
        {
            enemy.Hit(_player.Attack);
        }
    }

    public void Exit()
    {
        _player.NextAttack((Player.AttackState)_stateIndex);
        _player.StateChange(Player.MoveState.Normal);
    }

    public void FixedUpdate()
    {
    }

    public void Update()
    {
        _timer += Time.deltaTime;

        //コントローラーを使用する場合呼ばれない
        if (!_player.IsController)
        {
            var currentMouse = Mouse.current;
            if (currentMouse.leftButton.wasPressedThisFrame)
            {
                _stateIndex = (int)Player.AttackState.Attack1;
                Exit();
            }
        }

        //一定時間が経過したらコンボが途切れる
        if(_timer > _player.ComboInterval)
        {
            _stateIndex = (int)Player.AttackState.Idol;
            Exit();
        }
    }
}
