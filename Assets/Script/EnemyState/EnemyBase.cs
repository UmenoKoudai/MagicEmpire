using UnityEngine;

/// <summary>
/// エネミーの基底クラス
/// </summary>
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

    [SerializeField, Tooltip("アタック可能な範囲")]
    private float _attackRange;
    public float AttackRange => _attackRange;

    [SerializeField, Tooltip("移動先についたか判定する範囲")]
    private float _nextPointRange;
    public float NextPointRange => _nextPointRange;

    [Header("デバック用")]
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
    /// 初期化関数
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
    /// ステートを変更する
    /// </summary>
    /// <param name="value">変更したいステートのenum</param>
    public virtual void StateChange(EnemyState value)
    {
        State = value;
    }

    /// <summary>
    /// 指定した方向にゆっくり回転する関数
    /// </summary>
    /// <param name="target">回転させるTransform</param>
    /// <param name="direction">どの方向を向かせるか</param>
    /// <param name="errorRange">向かせたい方向に向いているかを判定する基準</param>
    /// <param name="rotateSpeed">回転させる速度</param>
    /// <returns></returns>
    public virtual bool SlerpRotation(Transform target, Quaternion direction, float errorRange = 0.5f, float rotateSpeed = 0.1f)
    {
        //既に指定の方向に向いている場合関数から抜ける
        if (Quaternion.Angle(target.rotation, direction) < errorRange) return false;

        //指定の方向に向くまで回転させる
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
