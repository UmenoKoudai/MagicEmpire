using Cinemachine;
using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour, IPose, IHit
{
    #region �V���A���C�Y
    [SerializeField]
    private bool _isController;
    public bool IsController => _isController;

    [Header("�L�����̓����Ɋւ���ݒ�")]
    [SerializeField, Tooltip("�v���C���[��HP")]
    private int _hp;
    public int HP
    {
        set
        {
            _hp = value;
            _hpSlider.fillAmount = 1 / _hp;
        }
    }

    [SerializeField, Tooltip("�U�����󂯂����̃G�t�F�N�g")]
    private GameObject _hitEffect;

    [SerializeField, Tooltip("HP��\������Image")]
    private Image _hpSlider;

    [SerializeField, Tooltip("�U����")]
    private int _attack;
    public int Attack => _attack;

    [SerializeField, Tooltip("�ړ����x")]
    private float _speed;
    public float Speed => _speed;

    [SerializeField, Tooltip("�R���{���r�؂�鎞��")]
    private float _comboInterval;
    public float ComboInterval => _comboInterval;
    [SerializeField]
    private float _hitStopTimer;

    [Header("�G�t�F�N�g")]
    [SerializeField, Tooltip("�U���G�t�F�N�g")]
    private ParticleSystem[] _slashEffect;
    public ParticleSystem[] SlashEffect => _slashEffect;

    [SerializeField, Tooltip("�_�b�V���̃G�t�F�N�g")]
    private ParticleSystem _dushEffect;
    public ParticleSystem DushEffect => _dushEffect;

    #endregion

    #region �폜�\��
    //[Header("�J�����Ɋւ���ݒ�")]
    //[SerializeField, Tooltip("�v���C���[�J����")]
    //private CinemachineVirtualCamera _playerCamera;
    //[SerializeField, Tooltip("�f�t�H���g��Y���W")]
    //private float _defaultPositionY = 5f;
    //public float DefaultPositionY => _defaultPositionY;
    //[SerializeField, Tooltip("�f�t�H���g��Y�p�x")]
    //private float _defaultRotationY = 2f;
    //public float DefaultRotationY => _defaultRotationY;
    //[SerializeField, Tooltip("�_�b�V������Y���W")]
    //private float _dushPositionY = 4f;
    //public float DushPositionY => _dushPositionY;
    //[SerializeField, Tooltip("�_�b�V������Y�p�x")]
    //private float _dushRotationY = 4f;
    //public float DushRotationY => _dushRotationY;
    #endregion

    #region�@��V���A���C�Y�ϐ�
    public Rigidbody Rb { get; set; }
    public Animator Anim { get; set; }
    public Vector3 MoveVector { get; set; }
    public Vector2 StepVector { get; set; }

    private List<EnemyBase> _inRangeEnemy;
    public List<EnemyBase> InRangeEnemy => _inRangeEnemy;

    private float _defaultSpeed;
    private float _defaultAnimSpeed;
    #endregion

    #region �ړ��̃X�e�[�g
    private IStateMachine _currentState;
    private IStateMachine[] _states = new IStateMachine[(int)MoveState.Max];

    private MoveState _nowState = MoveState.Normal;

    public MoveState State
    {
        set
        {
            if (_nowState == value) return;
            _nowState = value;
            if (_nowState == MoveState.Stop) return;
            _currentState = _states[(int)_nowState];
            _currentState.Enter();
        }
    }

    public enum MoveState
    {
        Stop,
        Normal,
        Dush,
        Step,

        Max,
    }
    #endregion

    #region �U���̃X�e�[�g
    private IStateMachine[] _attackState = new IStateMachine[(int)AttackState.Max];
    private IStateMachine _currentAttack;

    private AttackState _nowAttack = AttackState.Idol;
    public AttackState NowAttack
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
    #endregion

    void Start()
    {
        _inRangeEnemy = new List<EnemyBase>();
        Rb = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
        _states[(int)MoveState.Normal] = new NormalMove(this);
        _states[(int)MoveState.Dush] = new DushMove(this);
        _states[(int)MoveState.Step] = new StepMove(this);
        _attackState[(int)AttackState.Idol] = new Idol(this);
        _attackState[(int)AttackState.Attack1] = new Attack1(this);
        _attackState[(int)AttackState.Attack2] = new Attack2(this);
        _attackState[(int)AttackState.Attack3] = new Attack3(this);
        _currentState = _states[(int)_nowState];
        _currentAttack = _attackState[(int)_nowAttack];
        _currentAttack.Enter();
        Anim.speed = 1.5f;
        _hpSlider.fillAmount = 1;
    }

    void Update()
    { 
        _currentAttack.Update();
        if (_nowState == MoveState.Stop) return;
        _currentState.Update();
    }

    private void FixedUpdate()
    {
        _currentAttack.FixedUpdate();
        if (_nowState == MoveState.Stop) return;
        _currentState.FixedUpdate();
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
    /// �ړ��̃X�e�[�g��ς���֐�
    /// </summary>
    /// <param name="value">�ύX��̃X�e�[�g</param>
    public void StateChange(MoveState value)
    {
        State = value;
    }

    /// <summary>
    /// �U���̃X�e�[�g��ς���֐�
    /// </summary>
    /// <param name="value"></param>
    public void NextAttack(AttackState value) 
    {
        NowAttack = value;
    }

    /// <summary>
    /// �ꎞ��~�����Ƃ��ɌĂ΂��
    /// </summary>
    public void Pose()
    {
        _defaultSpeed = _speed;
        _defaultAnimSpeed = Anim.speed;
        _speed = 0;
        Anim.speed = 0;
    }

    /// <summary>
    /// �ꎞ��~�����������Ƃ��ɌĂ΂��
    /// </summary>
    public void Resume()
    {
        _speed = _defaultSpeed;
        Anim.speed = _defaultAnimSpeed;
    }

    /// <summary>
    /// �G�l�~�[�̍U���������������ɏ����~�߂�
    /// </summary>
    public void HitStop()
    {
        //var main = _slashEffect[(int)_nowAttack - 1].main;
        // var defaultSpeed = Anim.speed;
        //var defaultParticleSpeed = main.simulationSpeed;
        //Anim.speed = 0f;
        //main.simulationSpeed = 0f;

        //DOTween.Sequence().SetDelay(_hitStopTimer).AppendCallback(() =>
        //{
        //    Anim.speed = defaultSpeed;
        //    main.simulationSpeed = defaultParticleSpeed;
        //});
    }

    /// <summary>
    /// �����̍U���������������ɏ����~�߂�
    /// </summary>
    public void AttackStop()
    {
        var main = _slashEffect[(int)_nowAttack - 1].main;
        var defaultSpeed = Anim.speed;
        var defaultParticleSpeed = main.simulationSpeed;
        Anim.speed = 0f;
        main.simulationSpeed = 0f;

        DOTween.Sequence().SetDelay(_hitStopTimer).AppendCallback(() =>
        {
            Anim.speed = defaultSpeed;
            main.simulationSpeed = defaultParticleSpeed;
        });
    }

    /// <summary>
    /// �U�����󂯂����ɌĂ΂��
    /// </summary>
    /// <param name="damage">����̍U����</param>
    public void Hit(int damage)
    {
        _hp -= damage;
        Instantiate(_hitEffect, transform.position, Quaternion.identity);
    }

    /// <summary>
    /// �U���͈͂ɃG�l�~�[���������烊�X�g�ɉ�����
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out EnemyBase enemy))
        {
            _inRangeEnemy.Add(enemy);
        }
    }

    /// <summary>
    /// �G���U���͈͂���o���烊�X�g����O��
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out EnemyBase enemy))
        {
            _inRangeEnemy.Remove(enemy);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveVector = context.ReadValue<Vector3>();
        Debug.Log($"���͂��ꂽ{MoveVector}");
    }

    public void OnActionPressed()
    {
        var action = (IInputAction)_currentState;
        action.ActionPressed();
    }

    public void OnActionReleased() 
    {
        var action = (IInputAction)_currentState;
        action.ActionReleased();
    }

    public void OnStrongAttack()
    {
        var combo = (ICombo)_currentAttack;
        combo.StrongAttack();
    }

    public void OnWeakAttack()
    {
        var combo = (ICombo)_currentAttack;
        combo.WeakAttack();
    }
}
