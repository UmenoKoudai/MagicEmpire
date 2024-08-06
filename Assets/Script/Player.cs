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

    public float Speed => _speed;

    public Rigidbody Rb { get; set; }
    public Animator Anim {  get; set; }

    IStateMachine _currentState;
    IStateMachine[] _states = new IStateMachine[(int)MoveState.Max];

    private MoveState _nowState = MoveState.Move;

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
        Move,

        Max,
    }

    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
        _states[(int)MoveState.Move] = new Move(this);
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
}
