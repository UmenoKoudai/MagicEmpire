using Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("�L�����̓����Ɋւ���ݒ�")]
    [SerializeField, Tooltip("")]
    private float _speed;
    public float Speed => _speed;

    [Header("�e�Ɋւ���ݒ�")]
    [SerializeField, Tooltip("")]
    private Bullet _bulletObject;
    [SerializeField, Tooltip("�e�𔭎˂���ꏊ")]
    private Transform _muzzle;
    [SerializeField, Tooltip("�e�𔭎˂���Ԋu")]
    private float _bulletInterval;
    public float BulletInterval => _bulletInterval;

    [Header("�J�����Ɋւ���ݒ�")]
    [SerializeField, Tooltip("�v���C���[�J����")]
    private CinemachineVirtualCamera _playerCamera;
    [SerializeField, Tooltip("�f�t�H���g��Y���W")]
    private float _defaultPositionY = 5f;
    public float DefaultPositionY => _defaultPositionY;
    [SerializeField, Tooltip("�f�t�H���g��Y�p�x")]
    private float _defaultRotationY = 2f;
    public float DefaultRotationY => _defaultRotationY;
    [SerializeField, Tooltip("�_�b�V������Y���W")]
    private float _dushPositionY = 4f;
    public float DushPositionY => _dushPositionY;
    [SerializeField, Tooltip("�_�b�V������Y�p�x")]
    private float _dushRotationY = 4f;
    public float DushRotationY => _dushRotationY;
    [SerializeField, Tooltip("�_�b�V���̃G�t�F�N�g")]
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
    /// �X�e�[�g��ς���֐�
    /// </summary>
    /// <param name="value">�ύX��̃X�e�[�g</param>
    public void StateChange(MoveState value)
    {
        State = value;
    }
}
