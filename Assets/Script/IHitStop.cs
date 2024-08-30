/// <summary>
/// 攻撃がヒットした時に実行される
/// </summary>
public interface IHit
{
    /// <summary>
    /// ダメージを与える関数
    /// </summary>
    /// <param name="damage"></param>
    public abstract void Hit(int damage);

    /// <summary>
    /// ダメージを受けた時にヒットストップを実行される
    /// </summary>
    public abstract void HitStop();
}
