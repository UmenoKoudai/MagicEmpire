using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �G�l�~�[�̊��N���X
/// </summary>
public class EnemyBase : MonoBehaviour, IHit
{
    [SerializeField]
    private EnemyType _type;

    [SerializeField, Tooltip("�G�l�~�[��HP")]
    private int _hp;
    public int HP 
    { 
        get => _hp; 
        set
        {
            _hp = value;
            _hpSlider.value = _hp;
            if (_hp <= 0)
            {
                StateChange(EnemyState.EnemyDie);
                OnEnemyDestroy?.Invoke(_type);
            }
        }
    }

    [SerializeField]
    private Slider _hpSlider;

    [SerializeField]
    private ParticleSystem _hitEffect;
    public ParticleSystem HitEffect => _hitEffect;

    [SerializeField, Tooltip("�G�l�~�[�̍U����")]
    private int _attack;
    public int Attack => _attack;

    [SerializeField, Tooltip("�G�l�~�[�̈ړ����x")]
    private float _speed;
    public float Speed => _speed;

    [SerializeField, Tooltip("�G�𔭌�����͈�")]
    private float _serchRange;
    public float SerchRange => _serchRange;

    [SerializeField, Tooltip("�ړ��ł���͈�")]
    private float _moveRange;
    public float MoveRange => _moveRange;

    [SerializeField, Tooltip("�A�^�b�N�\�Ȕ͈�")]
    private float _attackRange;
    public float AttackRange => _attackRange;

    [SerializeField, Tooltip("�ړ���ɂ��������肷��͈�")]
    private float _nextPointRange;
    public float NextPointRange => _nextPointRange;

    [SerializeField]
    private Renderer _renderer;
    public Renderer EnemyRender => _renderer;

    [SerializeField]
    private float _hitStopTimer;
    public float HitStopTimer => _hitStopTimer;

    [Header("�f�o�b�N�p")]
    [SerializeField]
    private GameObject _marker;

    public event Action<EnemyType> OnEnemyDestroy;
    public enum AnimationState
    {
        Walk = 1,
        Idle,
        Run,
        Attack,
        Hit,
    }
    public enum EnemyType
    {
        Slime,
    }

    #region �G�l�~�[�̏�ԊǗ��X�e�[�g
    private IStateMachine[] _states = new IStateMachine[(int)EnemyState.Max];
    public IStateMachine[] States { get => _states; set => _states = value; }
    private IStateMachine _currentState;
    public IStateMachine CurrentState => _currentState;

    public Rigidbody Rb { get; set; }
    public Animator Anime { get; set; }

    public enum EnemyState
    {
        None,
        FreeMove,
        DiscoveryMove,
        ChaseMove,
        AttackMove,
        Enemyhit,
        EnemyDie,

        Max,
    }

    private EnemyState _state = EnemyState.None;
    public EnemyState State
    {
        get => _state;
        set
        {
            Debug.Log($"ChangeState:{value}");
            if (_state == value) return;
            _state = value;
            _currentState = _states[(int)_state];
            _currentState?.Enter();
        }
    }
    #endregion

    /// <summary>
    /// �������֐�
    /// </summary>
    public virtual void Init()
    {
        Debug.Log("Virtual");
        Rb = GetComponent<Rigidbody>();
        Anime = GetComponent<Animator>();
        _hpSlider.maxValue = _hp;
        _hpSlider.value = _hp;
    }

    public void ManualUpdate()
    {
        _currentState?.Update();
        Debug.Log($"{_currentState}");
    }

    public void ManualFixedUpdate()
    {
        _currentState?.FixedUpdate();
    }

    /// <summary>
    /// �X�e�[�g��ύX����
    /// </summary>
    /// <param name="value">�ύX�������X�e�[�g��enum</param>
    public virtual void StateChange(EnemyState value)
    {
        State = value;
    }

    /// <summary>
    /// �w�肵�������ɂ�������]����֐�
    /// </summary>
    /// <param name="target">��]������Transform</param>
    /// <param name="direction">�ǂ̕������������邩</param>
    /// <param name="errorRange">���������������Ɍ����Ă��邩�𔻒肷��</param>
    /// <param name="rotateSpeed">��]�����鑬�x</param>
    /// <returns></returns>
    public virtual bool SlerpRotation(Transform target, Quaternion direction, float errorRange = 0.5f, float rotateSpeed = 0.1f)
    {
        //���Ɏw��̕����Ɍ����Ă���ꍇ�֐����甲����
        if (Quaternion.Angle(target.rotation, direction) < errorRange) return false;

        //�w��̕����Ɍ����܂ŉ�]������
        while(Quaternion.Angle(target.rotation, direction) > errorRange)
        {
            target.rotation = Quaternion.Slerp(target.rotation, direction, rotateSpeed);
        }
        return true;
    }

    public void HitStop()
    {
        //Rb.velocity = Vector3.zero;
        //_defaultAnimeSpeed = Anime.speed;
        //Anime.speed = 0;
        //var dir = transform.position - _player.transform.position;
        //DOTween.Sequence().SetDelay(_hitStopTimer).AppendCallback(() =>
        //{
        //    Anime.speed = _defaultAnimeSpeed; 
        //    Rb.AddForce((dir.normalized) * 100f, ForceMode.Impulse);
        //});
        //Debug.Log("�U�����ꂽ");
    }

    /// <summary>
    /// �U�����󂯂����ɌĂ΂�邱����HP�����炷
    /// </summary>
    /// <param name="damage"></param>
    public void Hit(int damage)
    {
        if (_state == EnemyState.EnemyDie) return;
        StateChange(EnemyState.Enemyhit);
        HP -= damage;
        _hitEffect.Play();
    }

    public void EnemyDestroy()
    {
        Destroy(gameObject);
    }

    public void Log(string message)
    {
        Debug.Log(message);
    }
}
