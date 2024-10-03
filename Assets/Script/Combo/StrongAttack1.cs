using UnityEngine;

public class StrongAttack1 : IStateMachine, ICombo
{
    private Player _player;
    private float _timer;
    private int _stateIndex;

    public StrongAttack1(Player player)
    {
        _player = player;
    }

    public void StrongAttack()
    {
        _stateIndex = (int)Player.AttackState.StrongAttack2;
        Exit();
    }

    public void WeakAttack()
    {
        _stateIndex = (int)Player.AttackState.Attack1;
        Exit();
    }

    public void Enter()
    {
        _player.Anim.SetTrigger("StrongAttack");
        _player.StateChange(Player.MoveState.Stop);
        _player.Rb.velocity = Vector3.zero;
        _timer = 0;

        for (int i = 0; i < 3; i++)
        {
            foreach (var enemy in _player.InRangeEnemy)
            {
                enemy.Hit((int)(_player.Attack * 1.5f));
                enemy.HitStop();
            }
        }
    }

    public void Exit()
    {
        _player.NextAttack((Player.AttackState)_stateIndex);
        _player.StateChange(Player.MoveState.Normal);
    }

    public void FixedUpdate()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > _player.ComboInterval)
        {
            _stateIndex = (int)Player.AttackState.Idol;
            Exit();
        }
    }
}
