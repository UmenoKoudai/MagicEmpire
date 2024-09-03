using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField, Tooltip("")]
    private EnemyBase[] _sponeEnemy;

    [SerializeField, Tooltip("")]
    [SerializeReference, SubclassSelector]
    private IQuest[] _quests;

    [SerializeField, Tooltip("")]
    private Text _questContans;

    [SerializeField, Tooltip("")]
    private Text _clearCount;

    private IQuest _quest;

    private int _questIndex;
    public int QuestIndex
    {
        get => _questIndex;
        set
        {
            _questIndex = value;
            if (_questIndex < _quests.Length)
            {
                _quest = _quests[_questIndex];
                _questContans.text = _quest.Content;
                _clearCount.text = $"({_quest.DestroyCount}/{_quest.ClearCount})";
            }
            else
            {
                _questContans.text = "�N�G�X�g����";
                _clearCount.text = "";
            }
        }
    }

    void Start()
    {
        //�f���Q�[�g���Z�b�g����
        foreach(var enemy in _sponeEnemy)
        {
            enemy.OnEnemyDestroy += DestroyCount;
        }
        QuestIndex = 0;
    }

    /// <summary>
    /// �G�l�~�[���|���ꂽ��Ă΂��
    /// </summary>
    /// <param name="type"></param>
    private void DestroyCount(EnemyBase.EnemyType type)
    {
        if (type != _quest.Type) return;
        _quest.CountUp();
        if (_quest.Judge()) QuestIndex++;
    }
}
