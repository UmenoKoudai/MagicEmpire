using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// エネミーの基底クラス
/// </summary>
public class EnemyBase : MonoBehaviour, IHit
{
    [SerializeField]
    private EnemyType _type;
    public EnemyType Type => _type;

    [Header("エネミーのステータスに関する数値")]
    [SerializeField, Tooltip("エネミーのHP")]
    private int _hp;
    public int HP 
    { 
        get => _hp; 
        set
        {
            _hp = value;
            _hpSlider.value = _hp;
            if (_hp <= 0 && _state != EnemyState.EnemyDie)
            {
                _isDestroy = true;
                StateChange(EnemyState.EnemyDie);
                OnEnemyDestroy?.Invoke(this);
            }
        }
    }

    [SerializeField]
    private Slider _hpSlider;

    [SerializeField, Tooltip("エネミーの攻撃力")]
    private int _attack;
    public int Attack => _attack;

    [SerializeField, Tooltip("エネミーの移動速度")]
    private float _speed;
    public float Speed => _speed;

    [Header("エネミーの様々な範囲")]
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

    [Header("インターバル")]
    [SerializeField]
    private float _hitStopTimer;
    public float HitStopTimer => _hitStopTimer;

    [SerializeField]
    private float _attackInterval;
    public float AttackInterval => _attackInterval;


    [Header("エフェクト")]
    [SerializeField]
    private ParticleSystem _hitEffect;
    public ParticleSystem HitEffect => _hitEffect;

    [SerializeField]
    private ParticleSystem _dieEffect;
    public ParticleSystem DedieEffect => _dieEffect;

    public event Action<EnemyBase> OnEnemyDestroy;
    private int _defaultHp;

    [Header("敵を倒したときに非表示にするオブジェクト")]
    [SerializeField]
    private GameObject _destroyBody;
    public GameObject DestroyBody => _destroyBody;

    [SerializeField]
    private GameObject _hpBar;
    public GameObject HpBar => _hpBar;

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
        TurtleShell,
    }

    #region エネミーの状態管理ステート
    private IStateMachine[] _states = new IStateMachine[(int)EnemyState.Max];
    public IStateMachine[] States { get => _states; set => _states = value; }
    private IStateMachine _currentState;
    public IStateMachine CurrentState => _currentState;

    public Rigidbody Rb { get; set; }
    public Animator Anime { get; set; }
    private bool _isDestroy = false;
    public bool IsDestroy { get => _isDestroy; set => _isDestroy = value; }

    public enum EnemyState
    {
        None,
        FreeMove,
        DiscoveryMove,
        ChaseMove,
        AttackMove,
        Enemyhit,
        EnemyDie,
        EnemyRespone,

        Max,
    }

    private EnemyState _state = EnemyState.None;
    public EnemyState State
    {
        get => _state;
        set
        {
            if (_state == value) return;
            _state = value;
            _currentState = _states[(int)_state];
            _currentState?.Enter();
        }
    }
    #endregion

    /// <summary>
    /// 初期化関数
    /// </summary>
    public virtual void Init()
    {
        Rb = GetComponent<Rigidbody>();
        Anime = GetComponent<Animator>();
        _hpSlider.maxValue = _hp;
        _hpSlider.value = _hp;
        _defaultHp = _hp;
    }

    public void ManualUpdate()
    {
        Debug.Log(_state);
        _currentState?.Update();
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

    public void HitStop()
    {
        if (_state == EnemyState.EnemyDie) return;
        StateChange(EnemyState.Enemyhit);
    }

    /// <summary>
    /// 攻撃を受けた時に呼ばれるここでHPを減らす
    /// </summary>
    /// <param name="damage"></param>
    public void Hit(int damage)
    {
        if (_state == EnemyState.EnemyDie) return;
        HP -= damage;
        _hitEffect.Play();
    }

    public void Respone()
    {
        HP = _defaultHp;
    }

    public void Log(string message)
    {
        Debug.Log(message);
    }
}
