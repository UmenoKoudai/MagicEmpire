using Cinemachine;
using UnityEngine;

/// <summary>
/// �_�b�V���̈ړ��X�e�[�g
/// </summary>
public class DushMove : IStateMachine
{
    private Player _player;
    private float _h;
    private float _v;

    public DushMove(Player player)
    {
        _player = player;
    }

    public void Enter()
    {
        _player.DushEffect.gameObject.SetActive(true);
        _player.Transposer.m_FollowOffset.y = _player.DushPositionY;
        _player.Composer.m_TrackedObjectOffset.y = _player.DushRotationY;
    }

    public void Exit()
    {
        _player.StateChange(Player.MoveState.Normal);
    }

    public void FixedUpdate()
    {
        var dir = new Vector3(_h, 0, _v);
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;

        //�L�����Ɉړ��̓��͂�����ꍇ�̂݌�����ς���
        if (_h != 0 || _v != 0)
        {
            _player.transform.forward = dir;
        }

        _player.Rb.velocity = dir * _player.Speed * 2 + _player.Rb.velocity.y * Vector3.up;
    }

    public void Update()
    {
        _h = Input.GetAxis("Horizontal");
        _v = Input.GetAxis("Vertical");

        //���V�t�g�����𗣂�����ʏ�ړ��Ɉڍs����
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Exit();
        }

        //�������{�^���ɂ���Ė��@���g�p����
        if (Input.GetKeyDown(KeyCode.F1))
        {
            _player.MagicPlay(_player.AttackButton[(int)Player.ButtonNumber.Left]);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            _player.MagicPlay(_player.AttackButton[(int)Player.ButtonNumber.Right]);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            _player.MagicPlay(_player.AttackButton[(int)Player.ButtonNumber.Up]);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            _player.MagicPlay(_player.AttackButton[(int)Player.ButtonNumber.Down]);
        }
    }
}
