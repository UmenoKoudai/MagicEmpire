using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1 : IStateMachine, ICombo
{
    private Player _player;
    private EnemyBase _nearEnemy;
    private float _timer;
    private int _stateIndex;

    public Attack1(Player player)
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
        var distance = 0f;
        _player.Anim.SetTrigger("InplaceAttack");
        _player.StateChange(Player.MoveState.Stop);
        _player.SlashEffect[0].gameObject.SetActive(true);
        _timer = 0;

        //if (_player.InRangeEnemy.Count <= 0) return;
        ////ˆê”Ô‹ß‚¢“G‚ðŽæ“¾‚·‚é
        //foreach(var enemy in _player.InRangeEnemy)
        //{
        //    var d = Vector3.Distance(enemy.transform.position, _player.transform.position);
        //    if (distance > d)
        //    {
        //        distance = d;
        //        _nearEnemy = enemy;
        //    }
        //}

        //_player.transform.position = _nearEnemy.transform.position * 5;
    }

    public void Exit()
    {
        _player.NextAttack((Player.AttackState)_stateIndex);
        _player.StateChange(Player.MoveState.Normal);
        _player.SlashEffect[0].gameObject.SetActive(false);
    }

    public void FixedUpdate()
    {
    }

    public void Update()
    {
        _timer += Time.deltaTime;
        if(Input.GetButtonDown("Fire1"))
        {
            _stateIndex = (int)Player.AttackState.Attack2;
            Exit();
        }
        if(_timer > _player.ComboInterval)
        {
            _stateIndex = (int)Player.AttackState.Idol;
            Exit();
        }
    }
}
