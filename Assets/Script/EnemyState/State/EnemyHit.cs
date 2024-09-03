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
    private float _offset;

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
        //_enemy.HitEffect.gameObject.SetActive(true);

    }

    public void Exit()
    {
        _timer = 0;
        //_enemy.HitEffect.gameObject.SetActive(false);
        _enemy.StateChange(EnemyBase.EnemyState.FreeMove);
    }

    public void FixedUpdate()
    {
       
    }

    public void Update()
    {
        _offset = 2f * Time.deltaTime + Random.Range(0f, 100f);
        var noise = 2 * (Mathf.PerlinNoise(_offset, 0) - 0.5f);
        _enemy.transform.position = new Vector3(_enemy.transform.position.x, _enemy.transform.position.y, noise * 2);
        _timer += Time.deltaTime;
        if(_timer > _hitTimer)
        {
            _enemy.Anime.speed = _defaultAnimeSpeed;
            _enemy.Rb.velocity = Vector3.zero;
            _enemy.Rb.AddForce((_direction.normalized + Vector3.up) * 10f, ForceMode.Impulse);
            Exit();
        }
    }
}
