using Cinemachine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IPose, IHit
{
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

    [SerializeField]
    private int _attack;
    public int Attack => _attack;

    [Header("キャラの動きに関する設定")]
    [SerializeField, Tooltip("")]
    private float _speed;
    public float Speed => _speed;
    [SerializeField, Tooltip("コンボが途切れる時間")]
    private float _comboInterval;
    public float ComboInterval => _comboInterval;
    [SerializeField]
    private float _hitStopTimer;

    [Header("攻撃エフェクト")]
    [SerializeField]
    private ParticleSystem[] _slashEffect;
    public ParticleSystem[] SlashEffect => _slashEffect;

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
    public Animator Anim { get; set; }
    public CinemachineTransposer Transposer { get; set; }
    public CinemachineComposer Composer { get; set; }

    private List<EnemyBase> _inRangeEnemy;
    public List<EnemyBase> InRangeEnemy => _inRangeEnemy;

    private float _defaultSpeed;
    private float _defaultAnimSpeed;

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
        _attackState[(int)AttackState.Idol] = new Idol(this);
        _attackState[(int)AttackState.Attack1] = new Attack1(this);
        _attackState[(int)AttackState.Attack2] = new Attack2(this);
        _attackState[(int)AttackState.Attack3] = new Attack3(this);
        _currentState = _states[(int)_nowState];
        _currentAttack = _attackState[(int)_nowAttack];
        Transposer = _playerCamera.GetCinemachineComponent<CinemachineTransposer>();
        Composer = _playerCamera.GetCinemachineComponent<CinemachineComposer>();
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

    public void Step(Vector3 direction)
    {
        direction = direction.normalized;
        Anim.SetTrigger("StepTrigger");
        Anim.SetFloat("StepX", direction.x);
        Anim.SetFloat("StepY", direction.z);
        Rb.AddForce(direction * 10, ForceMode.Impulse);
        StateChange(MoveState.Normal);
    }

    public void Pose()
    {
        _defaultSpeed = _speed;
        _defaultAnimSpeed = Anim.speed;
        _speed = 0;
        Anim.speed = 0;
    }

    public void Resume()
    {
        _speed = _defaultSpeed;
        Anim.speed = _defaultAnimSpeed;
    }

    public void HitStop()
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

    public void Hit(int damage)
    {
        _hp -= damage;
        Instantiate(_hitEffect, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out EnemyBase enemy))
        {
            _inRangeEnemy.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out EnemyBase enemy))
        {
            _inRangeEnemy.Remove(enemy);
        }
    }
}
