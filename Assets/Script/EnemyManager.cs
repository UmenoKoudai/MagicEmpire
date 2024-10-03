using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    [SerializeField, Tooltip("�t�B�[���h�ɔz�u���Ă���G�l�~�[���i�[")]
    private EnemyBase[] _sponeEnemy;

    [SerializeField, Tooltip("�N�G�X�g���i�[����")]
    [SerializeReference, SubclassSelector]
    private IQuest[] _quests;

    [SerializeField, Tooltip("�N�G�X�g�̓��e��\������e�L�X�g")]
    private Text _questContans;

    [SerializeField, Tooltip("���݂̌��j����\������e�L�X�g")]
    private Text _clearCount;

    [SerializeField]
    private GameObject _inGameCanvas;

    [SerializeField]
    private GameObject _gameEndCanvas;

    [SerializeField]
    private float _responeInterval;

    [SerializeField]
    private SceneChange _sceneManager;

    private IQuest _quest;
    private List<EnemyBase> _destroyEnemys = new List<EnemyBase>();

    private int _questIndex;
    private float _timer;

    private int _destroyCount;
    public int DestroyCount
    {
        get => _destroyCount;
        set
        {
            _destroyCount = value;
            _clearCount.text = $"({_destroyCount}/{_quest.ClearCount})";
        }
    }

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
                GameEnd();
            }
        }
    }

    void Start()
    {
        //�f���Q�[�g���Z�b�g����
        foreach(var enemy in _sponeEnemy)
        {
            enemy.OnEnemyDestroy += Destroy;
        }
        QuestIndex = 0;
    }

    /// <summary>
    /// �G�l�~�[���|���ꂽ��Ă΂��
    /// </summary>
    /// <param name="type"></param>
    private void Destroy(EnemyBase enemy)
    {
        if (enemy.Type != _quest.Type) return;
        DestroyCount = _quest.CountUp();
        if (_quest.Judge()) QuestIndex++;
    }

    private void GameEnd()
    {
        _sceneManager.SceneSequence("Result");
    }
}
