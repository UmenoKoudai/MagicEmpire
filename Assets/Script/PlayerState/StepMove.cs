using UnityEngine;

public class StepMove : IStateMachine
{
    private Player _player;
    private float _timer;
    private int _stateIndex;

    public StepMove(Player player)
    {
        _player = player;
    }

    public void Enter()
    {
        _player.Anim.SetFloat("StepX", _player.StepVector.x);
        _player.Anim.SetFloat("StepY", _player.StepVector.y);
        _player.Anim.SetTrigger("StepTrigger");
        _player.Rb.AddForce(_player.MoveVector * 10f, ForceMode.Impulse);
        _timer = 0;
    }

    public void Exit()
    {
        _player.StateChange((Player.MoveState)_stateIndex);
    }

    public void FixedUpdate()
    {
    }

    public void Update()
    {
        _timer += Time.deltaTime;
        ////左シフトから手を離したら通常移動に移行する
        //if (Input.GetKeyUp(KeyCode.LeftShift))
        //{
        //    _stateIndex = (int)Player.MoveState.Normal;
        //    Exit();
        //}

        if(_timer > 1f)
        {
            _stateIndex = (int)Player.MoveState.Normal;
            Exit();
        }
    }
}
