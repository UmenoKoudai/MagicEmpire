using Cinemachine;
using UnityEngine;

/// <summary>
/// ダッシュの移動ステート
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

        //キャラに移動の入力がある場合のみ向きを変える
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

        //左シフトから手を離したら通常移動に移行する
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Exit();
        }

        //押したボタンによって魔法を使用する
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
