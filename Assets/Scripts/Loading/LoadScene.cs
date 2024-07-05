using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LoadScene 
{
    [SerializeField, Scene] private int _nextScene;
    private AsyncOperation _asyncOperation = null;

    public void Start()
    {
        if (_asyncOperation != null)
            return;

        _asyncOperation = SceneManager.LoadSceneAsync(_nextScene);
        _asyncOperation.allowSceneActivation = false;
    }

    public void End()
    {
        if (_asyncOperation == null)
            return;

        _asyncOperation.allowSceneActivation = true;
        _asyncOperation = null;
    }
}
