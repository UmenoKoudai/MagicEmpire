using Cinemachine;
using UnityEngine;

/// <summary>
/// ダッシュの移動ステート
/// </summary>
public class DushMove : IStateMachine, IInputAction
{
    private Player _player;
    private float _h;
    private float _v;

    public DushMove(Player player)
    {
        _player = player;
    }

    public void ActionPressed()
    {
        Debug.Log("Press無し");
    }

    public void ActionReleased()
    {
        Debug.Log("ダッシュ終了");
        Exit();
    }

    public void Enter()
    {
        _player.DushEffect.gameObject.SetActive(true);
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
        _player.Anim.SetFloat("Speed", _player.Rb.velocity.magnitude);

        if (_player.IsController)
        {
            _h = _player.MoveVector.x;
            _v = _player.MoveVector.z;
        }
        else
        {
            _h = Input.GetAxis("Horizontal");
            _v = Input.GetAxis("Vertical");
        }

        //左シフトから手を離したら通常移動に移行する
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Exit();
        }
    }
}
