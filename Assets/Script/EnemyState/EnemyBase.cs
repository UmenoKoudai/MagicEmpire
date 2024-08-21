using UnityEngine;

/// <summary>
/// �G�l�~�[�̊��N���X
/// </summary>
public class EnemyBase : MonoBehaviour
{
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

    [Header("�f�o�b�N�p")]
    [SerializeField]
    private GameObject _marker;
    

    public enum EnemyState
    {
        None,
        FreeMove,
        DiscoveryMove,
        ChaseMove,
        AttackMove,

        Max,
    }

    public enum AnimationState
    {
        Walk = 1,
        Idle,
        Run,
        Attack,
    }

    private IStateMachine[] _states = new IStateMachine[(int)EnemyState.Max];
    private IStateMachine _currentState;

    public Rigidbody Rb { get; set; }
    public Animator Anime { get; set; }

    private EnemyState _state = EnemyState.None;
    public EnemyState State
    {
        set
        {
            if (_state == value) return;
            _state = value;
            _currentState = _states[(int)_state];
            _currentState?.Enter();
        }
    }

    /// <summary>
    /// �������֐�
    /// </summary>
    public virtual void Init()
    {
        Rb = GetComponent<Rigidbody>();
        Anime = GetComponent<Animator>();
        var player = FindObjectOfType<Player>();
        _states[(int)EnemyState.FreeMove] = new FreeMove(this, player);
        _states[(int)EnemyState.DiscoveryMove] = new DiscoveryMove(this, player);
        _states[(int)EnemyState.ChaseMove] = new ChaseMove(this, player);
        _states[(int)EnemyState.AttackMove] = new AttackMove(this, player);
        State = EnemyState.FreeMove;
    }

    public void ManualUpdate()
    {
        _currentState?.Update();
        Debug.Log(_currentState?.ToString());
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

    public virtual void DebugLog(string log)
    {
        Debug.Log(log);
    }

    public virtual void MarkerCreate(Vector3 point)
    {
        Instantiate(_marker, point, Quaternion.identity);
    }
}
