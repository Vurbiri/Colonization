//Assets\Vurbiri\Runtime\Types\LoadScene.cs
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vurbiri
{
    [System.Serializable]
    public class LoadScene
    {
        [SerializeField] private int _scene;
        private AsyncOperation _asyncOperation = null;

        public LoadScene(int scene) => _scene = scene;

        public AsyncOperation Start(bool allowSceneActivation = false)
        {
            if (_asyncOperation == null)
            {
                _asyncOperation = SceneManager.LoadSceneAsync(_scene);
                _asyncOperation.allowSceneActivation = allowSceneActivation;
            }

            return _asyncOperation;
        }

        public AsyncOperation End()
        {
            if (_asyncOperation != null)
                _asyncOperation.allowSceneActivation = true;

            return _asyncOperation;
        }
    }
}
