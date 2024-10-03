using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{
    [SerializeField]
    private Image _fadePanel;

    private bool _isSequence = false;

    public async void SceneSequence(string sceneName)
    {
        Debug.Log("シーン遷移中");
        while (!_isSequence) {
            _fadePanel.DOFade(1f, 0.5f).OnComplete(() =>
            {
                Debug.Log("シーン遷移！");
                SceneManager.LoadScene(sceneName);
                _isSequence = true;
            });
            if (_isSequence) break;
            await UniTask.Delay(TimeSpan.FromSeconds(1));
        }
    }

    private void OnDisable()
    {
        DOTween.KillAll();
    }
}
