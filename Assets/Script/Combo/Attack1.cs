using UnityEngine;
using UnityEngine.InputSystem;

public class Attack1 : IStateMachine, ICombo
{
    private Player _player;
    private float _timer;
    private int _stateIndex;

    public Attack1(Player player)
    {
        _player = player;
    }

    public void StrongAttack()
    {
        _stateIndex = (int)Player.AttackState.StrongAttack1;
        Exit();
    }

    public void WeakAttack()
    {
        _stateIndex = (int)Player.AttackState.Attack2;
        Exit();
    }

    public void Enter()
    {
        _player.Anim.SetTrigger("WeakAttack");
        _player.StateChange(Player.MoveState.Stop);
        _timer = 0;
        _player.Rb.velocity = Vector3.zero;
        if (_player.InRangeEnemy.Count <= 0) return;
        foreach (var enemy in _player.InRangeEnemy)
        {
            enemy.Hit(_player.Attack);
            _player.AttackStop();
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
        if (!_player.IsController)
        {
            var currentMouse = Mouse.current;
            if (currentMouse.leftButton.wasPressedThisFrame)
            {
                _stateIndex = (int)Player.AttackState.Attack2;
                Exit();
            }
        }
        if(_timer > _player.ComboInterval)
        {
            _stateIndex = (int)Player.AttackState.Idol;
            Exit();
        }
    }
}
