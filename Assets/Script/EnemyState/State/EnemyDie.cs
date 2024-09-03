using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;

public class EnemyDie : IStateMachine
{
    private EnemyBase _enemy;
    private Renderer _renderer;
    private bool _dieAnimeFinish;

    public EnemyDie(EnemyBase enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        _enemy.Anime.SetTrigger("Die");
        _renderer = _enemy.EnemyRender;
    }

    public void Exit()
    {
    }

    public void FixedUpdate()
    {
    }

    public async void Update()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(2f));
        _renderer.material.DOFade(endValue: 0f, duration: 3f).OnComplete(() =>
        {
            _enemy.EnemyDestroy();
        });
    }

    public void AnimeFinish()
    {
        _dieAnimeFinish = true;
    }
}
