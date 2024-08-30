using DG.Tweening;
using UnityEngine;

public class EnemyDie : IStateMachine
{
    private EnemyBase _enemy;
    private Renderer _renderer;
    private float _alpha;

    public EnemyDie(EnemyBase enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        _enemy.Anime.SetTrigger("Die");
        _renderer = _enemy.GetComponentInChildren<Renderer>();
    }

    public void Exit()
    {
    }

    public void FixedUpdate()
    {
    }

    public void Update()
    {
        _renderer.material.DOFade(endValue: 0f, duration: 1f).OnComplete(() =>
        {
            _enemy.EnemyDestroy();
        });
        //_alpha = _renderer.material.color.a;
        //_alpha -= Time.deltaTime;
        //_renderer.material.color = new Color(_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, _alpha);
        //if(_renderer.material.color.a < 0)
        //{

        //}
    }
}
