using Cinemachine;
using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour, IPose, IHit
{
    #region シリアライズ
    [SerializeField]
    private bool _isController;
    public bool IsController => _isController;

    [Header("キャラの動きに関する設定")]
    [SerializeField, Tooltip("プレイヤーのHP")]
    private int _hp;
    public int HP
    {
        set
        {
            _hp = value;
            _hpSlider.fillAmount = 1 / _hp;
        }
    }

    [SerializeField, Tooltip("攻撃を受けた時のエフェクト")]
    private GameObject _hitEffect;

    [SerializeField, Tooltip("HPを表示するImage")]
    private Image _hpSlider;

    [SerializeField, Tooltip("攻撃力")]
    private int _attack;
    public int Attack => _attack;

    [SerializeField, Tooltip("移動速度")]
    private float _speed;
    public float Speed => _speed;

    [SerializeField, Tooltip("コンボが途切れる時間")]
    private float _comboInterval;
    public float ComboInterval => _comboInterval;
    [SerializeField]
    private float _hitStopTimer;

    [Header("エフェクト")]
    [SerializeField, Tooltip("攻撃エフェクト")]
    private ParticleSystem[] _slashEffect;
    public ParticleSystem[] SlashEffect => _slashEffect;

    [SerializeField, Tooltip("ダッシュのエフェクト")]
    private ParticleSystem _dushEffect;
    public ParticleSystem DushEffect => _dushEffect;

    #endregion

    #region 削除予定
    //[Header("カメラに関する設定")]
    //[SerializeField, Tooltip("プレイヤーカメラ")]
    //private CinemachineVirtualCamera _playerCamera;
    //[SerializeField, Tooltip("デフォルトのY座標")]
    //private float _defaultPositionY = 5f;
    //public float DefaultPositionY => _defaultPositionY;
    //[SerializeField, Tooltip("デフォルトのY角度")]
    //private float _defaultRotationY = 2f;
    //public float DefaultRotationY => _defaultRotationY;
    //[SerializeField, Tooltip("ダッシュ時のY座標")]
    //private float _dushPositionY = 4f;
    //public float DushPositionY => _dushPositionY;
    //[SerializeField, Tooltip("ダッシュ時のY角度")]
    //private float _dushRotationY = 4f;
    //public float DushRotationY => _dushRotationY;
    #endregion

    #region　非シリアライズ変数
    public Rigidbody Rb { get; set; }
    public Animator Anim { get; set; }
    public Vector3 MoveVector { get; set; }
    public Vector2 StepVector { get; set; }

    private List<EnemyBase> _inRangeEnemy;
    public List<EnemyBase> InRangeEnemy => _inRangeEnemy;

    private float _defaultSpeed;
    private float _defaultAnimSpeed;
    #endregion

    #region 移動のステート
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

    #region 攻撃のステート
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
    /// 魔法を使用するアビリティ関数を実行する
    /// </summary>
    /// <param name="ability">発動したい魔法</param>
    public void MagicPlay(IAbility ability)
    {
        ability.Ability(this);
    }

    /// <summary>
    /// 移動のステートを変える関数
    /// </summary>
    /// <param name="value">変更先のステート</param>
    public void StateChange(MoveState value)
    {
        State = value;
    }

    /// <summary>
    /// 攻撃のステートを変える関数
    /// </summary>
    /// <param name="value"></param>
    public void NextAttack(AttackState value) 
    {
        NowAttack = value;
    }

    /// <summary>
    /// 一時停止したときに呼ばれる
    /// </summary>
    public void Pose()
    {
        _defaultSpeed = _speed;
        _defaultAnimSpeed = Anim.speed;
        _speed = 0;
        Anim.speed = 0;
    }

    /// <summary>
    /// 一時停止を解除したときに呼ばれる
    /// </summary>
    public void Resume()
    {
        _speed = _defaultSpeed;
        Anim.speed = _defaultAnimSpeed;
    }

    /// <summary>
    /// エネミーの攻撃が当たった時に少し止める
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
    /// 自分の攻撃が当たった時に少し止める
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
    /// 攻撃を受けた時に呼ばれる
    /// </summary>
    /// <param name="damage">相手の攻撃力</param>
    public void Hit(int damage)
    {
        _hp -= damage;
        Instantiate(_hitEffect, transform.position, Quaternion.identity);
    }

    /// <summary>
    /// 攻撃範囲にエネミーが入ったらリストに加える
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
    /// 敵が攻撃範囲から出たらリストから外す
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
        Debug.Log($"入力された{MoveVector}");
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
