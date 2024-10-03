using UnityEngine;
using DG.Tweening;

public class EnemyRespone : IStateMachine
{
    private EnemyBase _enemy;
    private Vector3 _startPotision;
    private float _timer;

    public EnemyRespone(EnemyBase enemy)
    {
        _enemy = enemy;
        _startPotision = _enemy.transform.position;
    }

    public void Enter()
    {
        _enemy.HpBar.SetActive(false);
        _enemy.DestroyBody.SetActive(false);
        _enemy.Rb.velocity = Vector3.zero;
        _enemy.DedieEffect.gameObject.SetActive(false);
    }

    public void Exit()
    {
        Debug.Log("復活のエネミー");
        _enemy.IsDestroy = false;
        _enemy.Rb.velocity = Vector3.zero;
        _enemy.transform.position = _startPotision;
        _enemy.Respone();
        _enemy.HpBar.SetActive(true);
        _enemy.DestroyBody.SetActive(true);
        _enemy.StateChange(EnemyBase.EnemyState.FreeMove);
    }

    public void FixedUpdate()
    {
    }

    public void Update()
    {
        _timer += Time.deltaTime;
        if(_timer > 10)
        {
            Exit();
        }
    }
}
