using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CountUpQuest : IQuest
{
    [SerializeField, Tooltip("クエストで指定された敵")]
    private EnemyBase.EnemyType _type;
    public EnemyBase.EnemyType Type => _type;

    [SerializeField, Tooltip("クリアするためのカウント")]
    private int _clearCount;
    public int ClearCount => _clearCount;

    [SerializeField, Tooltip("クエストの内容")]
    private string _contents;
    public string Content => _contents;

    private int _destroyCount;
    public int DestroyCount => _destroyCount;

    /// <summary>
    /// クエストクリアの条件を達成したかを確認する
    /// </summary>
    /// <returns></returns>
    public bool Judge()
    {
        if (_clearCount <= _destroyCount) return true;
        return false;
    }

    /// <summary>
    /// 撃破数を増やす
    /// </summary>
    public int CountUp()
    {
        return ++_destroyCount;
    }
}
