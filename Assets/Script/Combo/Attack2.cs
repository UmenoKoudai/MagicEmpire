using UnityEngine;
using UnityEngine.InputSystem;

public class Attack2 : IStateMachine, ICombo
{
    private Player _player;
    private float _timer;
    private int _stateIndex;

    public Attack2(Player player)
    {
        _player = player;
    }

    public void StrongAttack()
    {
        throw new System.NotImplementedException();
    }

    public void WeakAttack()
    {
        _stateIndex = (int)Player.AttackState.Attack3;
        Exit();
    }


    public void Enter()
    {
        _player.Anim.SetTrigger("InplaceAttack");
        _player.StateChange(Player.MoveState.Stop);
        _player.SlashEffect[1].Play();
        _timer = 0;
    }

    public void Exit()
    {
        _player.NextAttack((Player.AttackState)_stateIndex);
        _player.StateChange(Player.MoveState.Normal);
        if (_player.InRangeEnemy.Count <= 0) return;
        foreach (var enemy in _player.InRangeEnemy)
        {
            enemy.Hit(_player.Attack);
        }
    }

    public void FixedUpdate()
    {
    }

    public void Update()
    {
        _timer += Time.deltaTime;

        //�R���g���[���[�d�l�̏ꍇ�͌Ă΂�Ȃ�
        if (!_player.IsController)
        {
            var currentMouse = Mouse.current;
            if (currentMouse.leftButton.wasPressedThisFrame)
            {
                _stateIndex = (int)Player.AttackState.Attack3;
                Exit();
            }
        }

        //���Ԃ���������R���{���r�؂��
        if(_timer > _player.ComboInterval)
        {
            _stateIndex = (int)Player.AttackState.Idol;
            Exit();
        }
    }
}
