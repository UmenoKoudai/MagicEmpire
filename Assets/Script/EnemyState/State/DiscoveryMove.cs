using DG.Tweening;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// �v���C���[�𔭌��������̃X�e�[�g
/// </summary>
public class DiscoveryMove : IStateMachine
{
    private EnemyBase _enemy;
    private Player _player;
    //private Vector3 _direction;
    private int _stateIndex;
    private Quaternion _direction;
    private int debug;

    public DiscoveryMove(EnemyBase enemy, Player player)
    {
        _enemy = enemy;
        _player = player;
    }

    public void Enter()
    {
        //�v���C���[�̕������v�Z����
        var direction = _player.transform.position - _enemy.transform.position;
        _direction = Quaternion.LookRotation(direction);
        _enemy.Rb.velocity = Vector3.zero;
        _enemy.Anime.SetBool("IsIdol", true);
        _enemy.Anime.SetBool("IsWalk", false);
        _enemy.Anime.SetBool("IsRun", false);
    }

    public void Exit()
    {
        _enemy.StateChange((EnemyBase.EnemyState)_stateIndex);
    }

    public void FixedUpdate()
    {
    }

    public void Update()
    {
        _enemy.Anime.SetFloat("MoveState", (int)EnemyBase.AnimationState.Idle);
        var direction = _player.transform.position - _enemy.transform.position;
        _direction = Quaternion.LookRotation(direction);

        ////���������v���C���[�̕��������������I�������Exit���Ă�
        if (_enemy.SlerpRotation(_enemy.transform, _direction))
        {
            _stateIndex = (int)EnemyBase.EnemyState.ChaseMove;
            Exit();
        }

        //�v���C���[���T�[�`�͈͂��痣�ꂽ��p�j�s���ɖ߂�
        var distance = Vector3.Distance(_player.transform.position, _enemy.transform.position);
        if(distance > _enemy.SerchRange)
        {
            _enemy.transform.DOKill();
            _stateIndex = (int)EnemyBase.EnemyState.FreeMove;
            Exit();
        }
    }
}
