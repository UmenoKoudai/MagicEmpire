/// <summary>
/// �U�����q�b�g�������Ɏ��s�����
/// </summary>
public interface IHit
{
    /// <summary>
    /// �_���[�W��^����֐�
    /// </summary>
    /// <param name="damage"></param>
    public abstract void Hit(int damage);

    /// <summary>
    /// �_���[�W���󂯂����Ƀq�b�g�X�g�b�v�����s�����
    /// </summary>
    public abstract void HitStop();
}
