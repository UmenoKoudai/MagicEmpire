public interface IQuest
{
    public EnemyBase.EnemyType Type { get; }
    public int ClearCount {  get;}
    public string Content { get; }
    public int DestroyCount { get; }
    /// <summary>
    /// �N�G�X�g�N���A�̏�����B�����������m�F����
    /// </summary>
    /// <returns></returns>
    public abstract bool Judge();

    /// <summary>
    /// ���j���𑝂₷
    /// </summary>
    public abstract int CountUp();
}
