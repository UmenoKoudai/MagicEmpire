using UnityEngine;

//ステージを徘徊するステート
public class FreeMove : IStateMachine
{
    private EnemyBase _enemy;
    private Player _player;
    private Vector3 _direction;
    private int _stateIndex;

    public FreeMove(EnemyBase enemy, Player player)
    {
        _enemy = enemy;
        _player = player;
    }

    public void Enter()
    {
        //最初に移動する位置を計算する
        _direction = GetDir();
    }

    public void Exit()
    {
        _enemy.StateChange((EnemyBase.EnemyState)_stateIndex);
    }

    public void FixedUpdate()
    {
        _enemy.Rb.velocity = _direction * _enemy.Speed + _enemy.Rb.velocity.y * Vector3.up;
    }

    public void Update()
    {
        //プレイヤーを発見したらチェイスステートに移動する
        var playerDistance = Vector3.Distance(_enemy.transform.position, _player.transform.position);
        if(playerDistance < _enemy.SerchRange)
        {
            _stateIndex = (int)EnemyBase.EnemyState.DiscoveryMove;
            Exit();
        }

        //ランダムな位置付近まで移動したら次の移動位置を計算する
        var nextPosDistance = Vector3.Distance(_enemy.transform.position, _direction);
        if(nextPosDistance < 0.5f)
        {
            _direction = GetDir();
        }
    }

    /// <summary>
    /// 指定した半径のランダムな位置を計算してreturnする
    /// </summary>
    /// <returns>移動方向</returns>
    private Vector3 GetDir()
    {
        var random = Random.Range(0, 361);
        var dir = new Vector3
            (Mathf.Sin(random) * _enemy.MoveRange + _enemy.transform.position.x,  //X座標の位置を計算
            _enemy.transform.position.y, 　　　　　　　　　　　　　　　　　　　　 //Y座標は変えないのでそのまま
            Mathf.Cos(random) * _enemy.MoveRange + _enemy.transform.position.z);　//Z座標の位置を計算
        return dir;
    }
}
