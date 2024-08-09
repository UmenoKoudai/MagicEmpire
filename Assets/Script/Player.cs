using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private Bullet _bulletObject;
    [SerializeField]
    private Transform _muzzle;
    [SerializeField]
    private float _bulletInterval;

    public float Speed => _speed;

    public Rigidbody Rb { get; set; }
    public Animator Anim {  get; set; }
    public float BulletInterval => _bulletInterval;

    IStateMachine _currentState;
    IStateMachine[] _states = new IStateMachine[(int)MoveState.Max];

    private MoveState _nowState = MoveState.Normal;

    public MoveState State
    {
        set 
        {
            if (_nowState == value) return;
            _nowState = value;
            _currentState = _states[(int)_nowState];
            _currentState.Enter();
        }
    }

    public enum MoveState
    {
        Normal,
        Dush,

        Max,
    }

    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
        _states[(int)MoveState.Normal] = new NormalMove(this);
        _states[(int)MoveState.Dush] = new DushMove(this);
        _currentState = _states[(int)_nowState];
    }

    void Update()
    {
        _currentState.Update();
    }

    private void FixedUpdate()
    {
        _currentState.FixedUpdate();
    }

    public void Shoot()
    { 
        var bullet =  Instantiate(_bulletObject, _muzzle.position, Quaternion.identity);
        bullet.Direction = transform.forward;
    }

    public void StateChange(MoveState value)
    {
        State = value;
    }
}
