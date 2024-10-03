using DG.Tweening;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// プレイヤーを発見した時のステート
/// </summary>
public class DiscoveryMove : IStateMachine
{
    private EnemyBase _enemy;
    private Player _player;
    //private Vector3 _direction;
    private int _stateIndex;
    private Quaternion _direction;
    private int debug;

    public DiscoveryMove(EnemyBase enemy, Player player)
    {
        _enemy = enemy;
        _player = player;
    }

    public void Enter()
    {
        //プレイヤーの方向を計算する
        var direction = _player.transform.position - _enemy.transform.position;
        _direction = Quaternion.LookRotation(direction);
        _enemy.Rb.velocity = Vector3.zero;
        _enemy.Anime.SetBool("IsIdol", true);
        _enemy.Anime.SetBool("IsWalk", false);
        _enemy.Anime.SetBool("IsRun", false);
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
        var direction = _player.transform.position - _enemy.transform.position;
        _direction = Quaternion.LookRotation(direction);

        ////発見したプレイヤーの方向を向く向き終わったらExitを呼ぶ
        if (_enemy.SlerpRotation(_enemy.transform, _direction))
        {
            _stateIndex = (int)EnemyBase.EnemyState.ChaseMove;
            Exit();
        }

        //プレイヤーがサーチ範囲から離れたら徘徊行動に戻る
        var distance = Vector3.Distance(_player.transform.position, _enemy.transform.position);
        if(distance > _enemy.SerchRange)
        {
            _enemy.transform.DOKill();
            _stateIndex = (int)EnemyBase.EnemyState.FreeMove;
            Exit();
        }
    }
}
