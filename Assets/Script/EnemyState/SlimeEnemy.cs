
using DG.Tweening;

/// <summary>
/// スライムのエネミー
/// </summary>
public class SlimeEnemy : EnemyBase
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

    //public override void Init()
    //{
        
    //}

    private void OnDisable()
    {
        DOTween.KillAll();
    }
}
