using System.Diagnostics;
using UnityEngine;

/// <summary>
/// �X���C���̃G�l�~�[
/// </summary>
public class SlimeEnemy : EnemyBase
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        ManualUpdate();
    }

    private void FixedUpdate()
    {
        ManualFixedUpdate();
    }

    public override void Init()
    {
        base.Init();
        var _player = FindObjectOfType<Player>();
        States[(int)EnemyState.FreeMove] = new FreeMove(this, _player);
        States[(int)EnemyState.DiscoveryMove] = new DiscoveryMove(this, _player);
        States[(int)EnemyState.ChaseMove] = new ChaseMove(this, _player);
        States[(int)EnemyState.AttackMove] = new AttackMove(this, _player);
        States[(int)EnemyState.Enemyhit] = new EnemyHit(this, _player, HitStopTimer);
        States[(int)EnemyState.EnemyDie] = new EnemyDie(this, _player);
        States[(int)EnemyState.EnemyRespone] = new EnemyRespone(this);
        State = EnemyState.FreeMove;
        CurrentState.Enter();
    }

    public void AnimationFinish()
    {
        UnityEngine.Debug.Log("�A�j���[�V�������I�����");
        var die = (EnemyDie)States[(int)EnemyState.EnemyDie];
        die.AnimeFinish();
    }
}
