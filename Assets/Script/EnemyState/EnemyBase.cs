using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// エネミーの基底クラス
/// </summary>
public class EnemyBase : MonoBehaviour, IHit
{

    [SerializeField, Tooltip("エネミーのHP")]
    private int _hp;
    public int HP 
    { 
        get => _hp; 
        set
        {
            _hp = value;
            _hpSlider.value = _hp;
            if (_hp < 0)
            {
                StateChange(EnemyState.EnemyDie);
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

    [SerializeField]
    private float _hitStopTimer;

    [Header("デバック用")]
    [SerializeField]
    private GameObject _marker;

    private Player _player;
    

    public enum AnimationState
    {
        Walk = 1,
        Idle,
        Run,
        Attack,
        Hit,
    }

    #region エネミーの状態管理ステート
    private IStateMachine[] _states = new IStateMachine[(int)EnemyState.Max];
    private IStateMachine _currentState;

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
        _player = FindObjectOfType<Player>();
        _states[(int)EnemyState.FreeMove] = new FreeMove(this, _player);
        _states[(int)EnemyState.DiscoveryMove] = new DiscoveryMove(this, _player);
        _states[(int)EnemyState.ChaseMove] = new ChaseMove(this, _player);
        _states[(int)EnemyState.AttackMove] = new AttackMove(this, _player);
        _states[(int)EnemyState.Enemyhit] = new EnemyHit(this, _player, _hitStopTimer);
        _states[(int)EnemyState.EnemyDie] = new EnemyDie(this);
        State = EnemyState.FreeMove;
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
        //Rb.velocity = Vector3.zero;
        //_defaultAnimeSpeed = Anime.speed;
        //Anime.speed = 0;
        //var dir = transform.position - _player.transform.position;
        //DOTween.Sequence().SetDelay(_hitStopTimer).AppendCallback(() =>
        //{
        //    Anime.speed = _defaultAnimeSpeed; 
        //    Rb.AddForce((dir.normalized) * 100f, ForceMode.Impulse);
        //});
        //Debug.Log("攻撃された");
    }

    /// <summary>
    /// 攻撃を受けた時に呼ばれるここでHPを減らす
    /// </summary>
    /// <param name="damage"></param>
    public void Hit(int damage)
    {
        if (_state == EnemyState.EnemyDie) return;
        HP -= damage;
        var effect = Resources.Load("HitEffect_A");
        Instantiate(effect, transform.position, Quaternion.identity);
        StateChange(EnemyState.Enemyhit);
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
