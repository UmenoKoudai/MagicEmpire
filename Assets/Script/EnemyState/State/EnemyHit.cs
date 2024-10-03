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
        _enemy.Anime.SetTrigger("EnemyHit");
        _enemy.Rb.velocity = Vector3.zero;
        _enemy.HitEffect.gameObject.SetActive(true);
        _timer = 0;
    }

    public void Exit()
    {
        _enemy.HitEffect.gameObject.SetActive(false);
        _enemy.StateChange(EnemyBase.EnemyState.FreeMove);
    }

    public void FixedUpdate()
    {
    }

    public void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > _hitTimer)
        {
            Exit();
        }
    }
}
