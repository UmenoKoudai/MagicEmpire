using DG.Tweening;
using UnityEngine;

/// <summary>
/// プレイヤーを攻撃するステート
/// </summary>
public class AttackMove : IStateMachine
{
    private EnemyBase _enemy;
    private Player _player;
    private float _timer;
    private int _stateIndex;

    public AttackMove(EnemyBase enemy, Player player)
    {
        _enemy = enemy;
        _player = player;
    }

    public void Enter()
    {
        _enemy.Rb.velocity = Vector3.zero;
    }

    public void Exit()
    {
        _enemy.StateChange((EnemyBase.EnemyState)_stateIndex);
    }

    public void FixedUpdate()
    {
    }

    public void Update()
    {
        _enemy.Anime.SetFloat("MoveState", (int)EnemyBase.AnimationState.Idle);
        var dir = _player.transform.position - _enemy.transform.position;
        //_enemy.transform.DORotate(dir, 0.5f, mode: RotateMode.Fast);
        _enemy.transform.forward = new Vector3(dir.x, _enemy.transform.position.y, dir.z);
        _timer += Time.deltaTime;
        if(_timer > 10)
        {
            _enemy.Anime.SetFloat("MoveState", (int)EnemyBase.AnimationState.Attack);
            Debug.Log("Attack");
            _timer = 0;
        }

        var distance = Vector3.Distance(_player.transform.position, _enemy.transform.position);
        if(distance > _enemy.AttackRange)
        {
            _enemy.transform.DOKill();
            _stateIndex = (int)EnemyBase.EnemyState.ChaseMove;
            Exit();
        }
        if(distance > _enemy.SerchRange)
        {
            _enemy.transform.DOKill();
            _stateIndex = (int)EnemyBase.EnemyState.FreeMove;
            Exit();
        }
    }
}
