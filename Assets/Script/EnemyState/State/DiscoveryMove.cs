using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�𔭌��������̃X�e�[�g
/// </summary>
public class DiscoveryMove : IStateMachine
{
    private EnemyBase _enemy;
    private Player _player;

    public DiscoveryMove(EnemyBase enemy, Player player)
    {
        _enemy = enemy;
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
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        throw new System.NotImplementedException();
    }
}
