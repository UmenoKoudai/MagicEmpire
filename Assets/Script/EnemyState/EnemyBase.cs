using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField, Tooltip("エネミーの移動速度")]
    private float _speed;
    public float Speed => _speed;

    [SerializeField, Tooltip("敵を発見する範囲")]
    private float _serchRange;
    public float SerchRange => _serchRange;

    [SerializeField, Tooltip("移動できる範囲")]
    private float _moveRange;
    public float MoveRange => _moveRange;

    public enum EnemyState
    {
        FreeMove,
        DiscoveryMove,
        ChaseMove,
        AttackMove,

        Max,
    }

    private IStateMachine[] _states = new IStateMachine[(int)EnemyState.Max];
    private IStateMachine _currentState;

    public EnemyState State
    {
        set
        {
            _currentState = _states[(int)value];
            _currentState?.Enter();
        }
    }

    private void Start()
    {
        //_states[(int)EnemyState.FreeMove] = new FreeMove();
        _currentState = _states[(int)EnemyState.FreeMove];
    }

    private void Update()
    {
        _currentState?.Update();
    }

    private void FixedUpdate()
    {
        _currentState?.FixedUpdate();
    }

    public virtual void StateChange(EnemyState value)
    {

    }
}
