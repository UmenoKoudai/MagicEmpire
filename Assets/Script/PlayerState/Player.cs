using Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("キャラの動きに関する設定")]
    [SerializeField, Tooltip("")]
    private float _speed;
    public float Speed => _speed;

    [Header("弾に関する設定")]
    [SerializeField, Tooltip("")]
    private Bullet _bulletObject;
    [SerializeField, Tooltip("弾を発射する場所")]
    private Transform _muzzle;
    [SerializeField, Tooltip("弾を発射する間隔")]
    private float _bulletInterval;
    public float BulletInterval => _bulletInterval;

    [Header("カメラに関する設定")]
    [SerializeField, Tooltip("プレイヤーカメラ")]
    private CinemachineVirtualCamera _playerCamera;
    [SerializeField, Tooltip("デフォルトのY座標")]
    private float _defaultPositionY = 5f;
    public float DefaultPositionY => _defaultPositionY;
    [SerializeField, Tooltip("デフォルトのY角度")]
    private float _defaultRotationY = 2f;
    public float DefaultRotationY => _defaultRotationY;
    [SerializeField, Tooltip("ダッシュ時のY座標")]
    private float _dushPositionY = 4f;
    public float DushPositionY => _dushPositionY;
    [SerializeField, Tooltip("ダッシュ時のY角度")]
    private float _dushRotationY = 4f;
    public float DushRotationY => _dushRotationY;
    [SerializeField, Tooltip("ダッシュのエフェクト")]
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
