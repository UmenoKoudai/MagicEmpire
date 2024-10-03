using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CountUpQuest : IQuest
{
    [SerializeField, Tooltip("�N�G�X�g�Ŏw�肳�ꂽ�G")]
    private EnemyBase.EnemyType _type;
    public EnemyBase.EnemyType Type => _type;

    [SerializeField, Tooltip("�N���A���邽�߂̃J�E���g")]
    private int _clearCount;
    public int ClearCount => _clearCount;

    [SerializeField, Tooltip("�N�G�X�g�̓��e")]
    private string _contents;
    public string Content => _contents;

    private int _destroyCount;
    public int DestroyCount => _destroyCount;

    /// <summary>
    /// �N�G�X�g�N���A�̏�����B�����������m�F����
    /// </summary>
    /// <returns></returns>
    public bool Judge()
    {
        if (_clearCount <= _destroyCount) return true;
        return false;
    }

    /// <summary>
    /// ���j���𑝂₷
    /// </summary>
    public int CountUp()
    {
        return ++_destroyCount;
    }
}
