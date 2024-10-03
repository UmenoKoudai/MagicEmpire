using UnityEngine;

/// <summary>
/// プレイヤーを追いかけるステート
/// </summary>
public class ChaseMove : IStateMachine
{
    private EnemyBase _enemy;
    private Player _player;
    private Vector3 _direction;
    private int _stateIndex;

    public ChaseMove(EnemyBase enemy, Player player)
    {
        _enemy = enemy;
        _player = player;
    }

    public void Enter()
    {
        _enemy.Anime.SetBool("IsRun", true);
        _enemy.Anime.SetBool("IsIdol", false);
        _enemy.Anime.SetBool("IsWalk", false);
    }

    public void Exit()
    {
        _enemy.StateChange((EnemyBase.EnemyState)_stateIndex);
    }

    public void FixedUpdate()
    {
        _enemy.Rb.velocity = _direction.normalized * (_enemy.Speed * 1.5f) + _enemy.Rb.velocity.y * Vector3.up;
    }

    public void Update()
    {
        _enemy.Anime.SetFloat("MoveState", (int)EnemyBase.AnimationState.Run);
        _direction = _player.transform.position - _enemy.transform.position;
        _enemy.transform.forward = _direction;
        var distance = Vector3.Distance(_player.transform.position, _enemy.transform.position);
        if(distance < _enemy.AttackRange)
        {
            _stateIndex = (int)EnemyBase.EnemyState.AttackMove;
            Exit();
        }
        if(distance > _enemy.SerchRange)
        {
            _stateIndex = (int)EnemyBase.EnemyState.FreeMove;
            Exit();
        }
    }
}
