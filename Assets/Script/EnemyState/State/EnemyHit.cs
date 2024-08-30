using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyHit : IStateMachine
{
    private EnemyBase _enemy;
    private Player _player;
    private Vector3 _direction;
    private float _defaultAnimeSpeed;
    private float _hitTimer;
    private float _timer;

    public EnemyHit(EnemyBase enemy, Player player, float hitTimer)
    {
        _enemy = enemy;
        _player = player;
        _hitTimer = hitTimer;
    }

    public void Enter()
    {
        _enemy.Anime.SetFloat("MoveState", (int)EnemyBase.AnimationState.Hit);
        _enemy.Rb.velocity = Vector3.zero;
        _defaultAnimeSpeed = _enemy.Anime.speed;
        _enemy.Anime.speed = 0;
        _direction = _enemy.transform.position - _player.transform.position;
    }

    public void Exit()
    {
        _enemy.StateChange(EnemyBase.EnemyState.FreeMove);
    }

    public void FixedUpdate()
    {
       
    }

    public void Update()
    {
        _timer += Time.deltaTime;
        if(_timer > _hitTimer)
        {
            _enemy.Anime.speed = _defaultAnimeSpeed;
            _enemy.Rb.velocity = Vector3.zero;
            _enemy.Rb.AddForce((_direction.normalized) * 100f, ForceMode.Impulse);
            Exit();
        }
    }
}
