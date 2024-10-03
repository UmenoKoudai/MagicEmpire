public interface IQuest
{
    public EnemyBase.EnemyType Type { get; }
    public int ClearCount {  get;}
    public string Content { get; }
    public int DestroyCount { get; }
    /// <summary>
    /// クエストクリアの条件を達成したかを確認する
    /// </summary>
    /// <returns></returns>
    public abstract bool Judge();

    /// <summary>
    /// 撃破数を増やす
    /// </summary>
    public abstract int CountUp();
}
