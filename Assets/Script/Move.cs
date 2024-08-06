using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : IStateMachine
{
    private Player _player;
    private float _h;
    private float _v;

    public Move(Player player)
    {
        _player = player;
    }

    public void Enter()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }

    public void FixedUpdate()
    {
        var dir  = Vector3.forward * _v + Vector3.right * _h;
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
        if(Input.GetButtonDown("Fire1"))
        {
            _player.Shoot();
        }
    }
}
