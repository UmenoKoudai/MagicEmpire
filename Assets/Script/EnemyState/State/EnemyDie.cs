using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;

public class EnemyDie : IStateMachine
{
    private EnemyBase _enemy;
    private Player _player;
    private float _defaultAnimeSpeed;
    private Vector3 _direstion;
    private float _timer;
    private bool _dieAnimeFinish;
    private float _defaultTimeScale;


    public EnemyDie(EnemyBase enemy, Player player)
    {
        _enemy = enemy;
        _player = player;
    }

    public void Enter()
    {
        _enemy.Anime.SetTrigger("Die");
        _defaultTimeScale = Time.timeScale;
        Time.timeScale = 0.3f;
        _direstion = _enemy.transform.position - _player.transform.position;
        _enemy.DedieEffect.gameObject.SetActive(true);
    }

    public void Exit()
    {
        _enemy.StateChange(EnemyBase.EnemyState.EnemyRespone);
    }

    public void FixedUpdate()
    {
    }

    public async void Update()
    {
        _timer += Time.deltaTime;
        if(_timer > 0.7f)
        {
            Time.timeScale = 1;
            _enemy.Rb.AddForce(_direstion * 50f, ForceMode.Impulse);
            _player.ResetAnimation();
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            Exit();
        }
    }

    public void AnimeFinish()
    {
        _dieAnimeFinish = true;
    }
}
