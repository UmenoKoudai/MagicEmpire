using Cinemachine;
using UnityEngine;

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

    public void Exit(Player.MoveState change)
    {
        _player.StateChange(change);
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

        _player.Rb.velocity = dir * 20 + _player.Rb.velocity.y * Vector3.up;
    }

    public void Update()
    {
        _h = Input.GetAxis("Horizontal");
        _v = Input.GetAxis("Vertical");

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Exit(Player.MoveState.Normal);
        }
    }
}
