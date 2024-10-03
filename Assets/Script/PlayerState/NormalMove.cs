using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �ʏ�̈ړ��X�e�[�g
/// </summary>
public class NormalMove : IStateMachine, IInputAction
{
    private Player _player;
    private float _h;
    private float _v;
    private int _state;

    public NormalMove(Player player)
    {
        _player = player;
    }

    public void ActionPressed()
    {
        Debug.Log("�m�[�}�����[�u");
        _state = (int)Player.MoveState.Dush;
        Exit();
    }

    public void ActionReleased()
    {
        Debug.Log("�����[�X����");
    }

    public void Enter()
    {
        _player.DushEffect.gameObject.SetActive(false);
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
        _player.Anim.SetFloat("Speed", _player.Rb.velocity.magnitude);
        if (_player.IsController)
        {
            _h = _player.MoveVector.x;
            _v = _player.MoveVector.z;
        }
        else
        {
            _h = Input.GetAxisRaw("Horizontal");
            _v = Input.GetAxisRaw("Vertical");
            var currentKey = Keyboard.current;
            if (currentKey.leftShiftKey.wasPressedThisFrame)
            {
                _state = (int)Player.MoveState.Dush;
                Exit();
            }
        }

        //���V�t�g��������_�b�V���ړ��Ɉȍ~

        // �X�e�b�v�@�\�\�聫
        //if(Input.GetButtonDown("Fire2"))
        //{
        //    _player.MoveVector = _direction;
        //    _player.StepVector = new Vector2(_h, _v);
        //    _state = (int)Player.MoveState.Step;
        //    Exit();
        //}
    }
}
