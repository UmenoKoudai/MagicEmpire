using Cysharp.Threading.Tasks;
using System;
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

    public async void Enter()
    {
        _player.Anim.SetTrigger("InplaceAttack");
        _player.StateChange(Player.MoveState.Stop);
        _player.SlashEffect[2].gameObject.SetActive(true);
        _timer = 0;
        await UniTask.Delay(TimeSpan.FromSeconds(0.08f));
        _player.SlashEffect[3].gameObject.SetActive(true);
    }

    public void Exit()
    {
        _player.NextAttack((Player.AttackState)_stateIndex);
        _player.StateChange(Player.MoveState.Normal);
        _player.SlashEffect[2].gameObject.SetActive(false); 
        _player.SlashEffect[3].gameObject.SetActive(false);
    }

    public void FixedUpdate()
    {
    }

    public void Update()
    {
        _timer += Time.deltaTime;
        if(Input.GetButtonDown("Fire1"))
        {
            _stateIndex = (int)Player.AttackState.Attack1;
            Exit();
        }
        if(_timer > _player.ComboInterval)
        {
            _stateIndex = (int)Player.AttackState.Idol;
            Exit();
        }
    }
}
