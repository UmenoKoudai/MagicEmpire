using Cinemachine;
using System.Xml.Serialization;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("�L�����̓����Ɋւ���ݒ�")]
    [SerializeField, Tooltip("")]
    private float _speed;
    public float Speed => _speed;
    [SerializeField, Tooltip("�R���{���r�؂�鎞��")]
    private float _comboInterval;
    public float ComboInterval => _comboInterval;
    #region
    //[Header("�G�t�F�N�g�̏o���ꏊ")]
    //[SerializeField, Tooltip("�E��̔��ˏꏊ")]
    //private Transform _muzzleRight;
    //public Transform MuzzleRight => _muzzleRight;
    //[SerializeField, Tooltip("����̔��ˏꏊ")]
    //private Transform _muzzleLeft;
    //public Transform MuzzleLeft => _muzzleLeft;
    //[SerializeField, Tooltip("�v���C���[�̒��S")]
    //private Transform _muzzleCenter;
    //public Transform MuzleCenter => _muzzleCenter;
    //[SerializeField, Tooltip("�e�𔭎˂���Ԋu")]
    //private float _bulletInterval;
    //public float BulletInterval => _bulletInterval;
    #endregion

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
    public Animator Anim { get; set; }
    public CinemachineTransposer Transposer { get; set; }
    public CinemachineComposer Composer { get; set; }

    private IStateMachine _currentState;
    private IStateMachine[] _states = new IStateMachine[(int)MoveState.Max];

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

    #region
    private IAbility[] _attackButton = new IAbility[(int)ButtonNumber.Max];
    public IAbility[] AttackButton { get => _attackButton; set => _attackButton = value; }

    public IAbility SelectMagic { get; set; }

    public enum ButtonNumber
    {
        Left,
        Right,
        Up,
        Down,

        Max,
    }
    #endregion

    public enum MoveState
    {
        Normal,
        Dush,

        Max,
    }

    private IStateMachine[] _attackState = new IStateMachine[(int)AttackState.Max];
    private IStateMachine _currentAttack;

    private AttackState _nowAttack = AttackState.Idol;
    public AttackState Attack
    {
        set
        {
            if(_nowAttack == value) return;
            _nowAttack = value;
            _currentAttack = _attackState[(int)_nowAttack];
            _currentAttack.Enter();
        }
    }

    public enum AttackState
    {
        Idol,
        Attack1,
        Attack2,
        Attack3,

        Max,
    }

    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
        _states[(int)MoveState.Normal] = new NormalMove(this);
        _states[(int)MoveState.Dush] = new DushMove(this);
        _attackState[(int)AttackState.Idol] = new Idol(this);
        _attackState[(int)AttackState.Attack1] = new Attack1(this);
        _attackState[(int)AttackState.Attack2] = new Attack2(this);
        _attackState[(int)AttackState.Attack3] = new Attack3(this);
        _currentState = _states[(int)_nowState];
        _currentAttack = _attackState[(int)_nowAttack];
        Transposer = _playerCamera.GetCinemachineComponent<CinemachineTransposer>();
        Composer = _playerCamera.GetCinemachineComponent<CinemachineComposer>();
        _currentAttack.Enter();

    }

    void Update()
    {
        _currentState.Update();
        _currentAttack.Update();
        Debug.Log(_currentAttack);
    }

    private void FixedUpdate()
    {
        _currentState.FixedUpdate();
        _currentAttack.FixedUpdate();
    }

    /// <summary>
    /// ���@���g�p����A�r���e�B�֐������s����
    /// </summary>
    /// <param name="ability">�������������@</param>
    public void MagicPlay(IAbility ability)
    {
        ability.Ability(this);
    }

    /// <summary>
    /// �X�e�[�g��ς���֐�
    /// </summary>
    /// <param name="value">�ύX��̃X�e�[�g</param>
    public void StateChange(MoveState value)
    {
        State = value;
    }

    public void NextAttack(AttackState value) 
    {
        Attack = value;
    }
}
