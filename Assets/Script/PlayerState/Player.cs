using Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("キャラの動きに関する設定")]
    [SerializeField]
    private float _speed;
    public float Speed => _speed;

    [Header("弾に関する設定")]
    [SerializeField]
    private Bullet _bulletObject;
    [SerializeField]
    private Transform _muzzle;
    [SerializeField]
    private float _bulletInterval;
    public float BulletInterval => _bulletInterval;

    [Header("カメラに関する設定")]
    [SerializeField]
    private CinemachineVirtualCamera _playerCamera;
    [SerializeField]
    private float _defaultPositionY = 5f;
    public float DefaultPositionY => _defaultPositionY;
    [SerializeField]
    private float _defaultRotationY = 2f;
    public float DefaultRotationY => _defaultRotationY;
    [SerializeField]
    private float _dushPositionY = 4f;
    public float DushPositionY => _dushPositionY;
    [SerializeField]
    private float _dushRotationY = 4f;
    public float DushRotationY => _dushRotationY;
    [SerializeField]
    private ParticleSystem _dushEffect;
    public ParticleSystem DushEffect => _dushEffect;


    public Rigidbody Rb { get; set; }
    public Animator Anim {  get; set; }
    public CinemachineTransposer Transposer { get; set; }
    public CinemachineComposer Composer { get; set; }

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
        Transposer = _playerCamera.GetCinemachineComponent<CinemachineTransposer>();
        Composer = _playerCamera.GetCinemachineComponent<CinemachineComposer>();
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

    /// <summary>
    /// ステートを変える関数
    /// </summary>
    /// <param name="value">変更先のステート</param>
    public void StateChange(MoveState value)
    {
        State = value;
    }
}
