using UnityEngine;

/// <summary>
/// �ʏ�̈ړ��X�e�[�g
/// </summary>
public class NormalMove : IStateMachine
{
    private Player _player;
    private float _h;
    private float _v;
    private float _timer;
    private int _state;

    public NormalMove(Player player)
    {
        _player = player;
    }

    public void Enter()
    {
        _player.DushEffect.gameObject.SetActive(false);
        _player.Transposer.m_FollowOffset.y = _player.DefaultPositionY;
        _player.Composer.m_TrackedObjectOffset.y = _player.DefaultRotationY;
    }

    public void Exit()
    {
        _player.StateChange((Player.MoveState)_state);
    }

    public void FixedUpdate()
    {
        var dir = new Vector3(_h, 0, _v);
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        if (_h != 0 || _v != 0)
        {
            _player.transform.forward = dir;
        }
        _player.Rb.velocity = dir.normalized * _player.Speed + _player.Rb.velocity.y * Vector3.up;
    }

    public void Update()
    {
        _h = Input.GetAxis("Horizontal");
        _v = Input.GetAxis("Vertical");
        _player.Anim.SetFloat("Speed", _player.Rb.velocity.magnitude);

        //���V�t�g��������_�b�V���ړ��Ɉȍ~
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _state = (int)Player.MoveState.Dush;
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
