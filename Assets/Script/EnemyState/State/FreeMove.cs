using UnityEngine;

//�X�e�[�W��p�j����X�e�[�g
public class FreeMove : IStateMachine
{
    private EnemyBase _enemy;
    private Player _player;
    private Vector3 _direction;
    private int _stateIndex;

    public FreeMove(EnemyBase enemy, Player player)
    {
        _enemy = enemy;
        _player = player;
    }

    public void Enter()
    {
        //�ŏ��Ɉړ�����ʒu���v�Z����
        _direction = GetDir();
    }

    public void Exit()
    {
        _enemy.StateChange((EnemyBase.EnemyState)_stateIndex);
    }

    public void FixedUpdate()
    {
        _enemy.Rb.velocity = _direction * _enemy.Speed + _enemy.Rb.velocity.y * Vector3.up;
    }

    public void Update()
    {
        //�v���C���[�𔭌�������`�F�C�X�X�e�[�g�Ɉړ�����
        var playerDistance = Vector3.Distance(_enemy.transform.position, _player.transform.position);
        if(playerDistance < _enemy.SerchRange)
        {
            _stateIndex = (int)EnemyBase.EnemyState.DiscoveryMove;
            Exit();
        }

        //�����_���Ȉʒu�t�߂܂ňړ������玟�̈ړ��ʒu���v�Z����
        var nextPosDistance = Vector3.Distance(_enemy.transform.position, _direction);
        if(nextPosDistance < 0.5f)
        {
            _direction = GetDir();
        }
    }

    /// <summary>
    /// �w�肵�����a�̃����_���Ȉʒu���v�Z����return����
    /// </summary>
    /// <returns>�ړ�����</returns>
    private Vector3 GetDir()
    {
        var random = Random.Range(0, 361);
        var dir = new Vector3
            (Mathf.Sin(random) * _enemy.MoveRange + _enemy.transform.position.x,  //X���W�̈ʒu���v�Z
            _enemy.transform.position.y, �@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@ //Y���W�͕ς��Ȃ��̂ł��̂܂�
            Mathf.Cos(random) * _enemy.MoveRange + _enemy.transform.position.z);�@//Z���W�̈ʒu���v�Z
        return dir;
    }
}
