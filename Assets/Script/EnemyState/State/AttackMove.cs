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
        _enemy.Anime.SetBool("IsIdol", true);
        _enemy.Anime.SetBool("IsRun", false);
        _enemy.Anime.SetBool("IsWalk", false);
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
        var dir = _player.transform.position - _enemy.transform.position;
        _enemy.transform.forward = new Vector3(dir.x, 0, dir.z);
        _timer += Time.deltaTime;
        if(_timer > _enemy.AttackInterval)
        {
            _enemy.Anime.SetTrigger("Attack");
            _player.Hit(_enemy.Attack);
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
