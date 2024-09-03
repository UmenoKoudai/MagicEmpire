using UnityEngine;
using UnityEngine.InputSystem;

public class Idol : IStateMachine, ICombo
{
    private Player _player;
    private float _timer;
    private int _stateIndex;

    public Idol(Player player)
    {
        _player = player;
    }

    public void StrongAttack()
    {
        throw new System.NotImplementedException();
    }

    public void WeakAttack()
    {
        _stateIndex = (int)Player.AttackState.Attack1;
        Exit();
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
        if (!_player.IsController)
        {
            var currentMouse = Mouse.current;
            if (currentMouse.leftButton.wasPressedThisFrame)
            {
                _stateIndex = (int)Player.AttackState.Attack1;
                Exit();
            }
        }
    }
}
