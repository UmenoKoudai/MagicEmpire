using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleShellEnemy : EnemyBase
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        ManualUpdate();
    }

    private void FixedUpdate()
    {
        ManualFixedUpdate();
    }

    public override void Init()
    {
        base.Init();
        var _player = FindObjectOfType<Player>();
        States[(int)EnemyState.FreeMove] = new FreeMove(this, _player);
        States[(int)EnemyState.DiscoveryMove] = new DiscoveryMove(this, _player);
        States[(int)EnemyState.ChaseMove] = new ChaseMove(this, _player);
        States[(int)EnemyState.AttackMove] = new AttackMove(this, _player);
        States[(int)EnemyState.Enemyhit] = new EnemyHit(this, _player, HitStopTimer);
        States[(int)EnemyState.EnemyDie] = new EnemyDie(this, _player);
        States[(int)EnemyState.EnemyRespone] = new EnemyRespone(this);
        State = EnemyState.FreeMove;
        CurrentState.Enter();
    }
}
