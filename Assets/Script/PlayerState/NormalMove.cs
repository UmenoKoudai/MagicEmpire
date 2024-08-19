using Cinemachine;
using UnityEngine;

public class NormalMove : IStateMachine
{
    private Player _player;
    private float _h;
    private float _v;
    private float _timer;

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

    public void Exit(Player.MoveState change)
    {
        _player.StateChange(change);
    }

    public void FixedUpdate()
    {
        var dir  = new Vector3(_h, 0, _v);
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        if(_h != 0 || _v != 0)
        {
            _player.transform.forward = dir;
        }
        _player.Rb.velocity = dir.normalized * 10 + _player.Rb.velocity.y * Vector3.up;
    }

    public void Update()
    {
        _h = Input.GetAxis("Horizontal");
        _v = Input.GetAxis("Vertical");
        _player.Anim.SetFloat("Speed", _player.Rb.velocity.magnitude);

        //左シフト押したらダッシュ移動に以降
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            Exit(Player.MoveState.Dush);
        }

        //下仮で弾を撃つ仕組み
        if(Input.GetButton("Fire1"))
        {
            _timer += Time.deltaTime;
            if (_player.BulletInterval < _timer)
            {
                _player.Shoot();
                _player.Anim.SetBool("Attack1", true);
                _timer = 0;
            }
        }
        else
        {
            _player.Anim.SetBool("Attack1", false);
        }
        //
    }
}
