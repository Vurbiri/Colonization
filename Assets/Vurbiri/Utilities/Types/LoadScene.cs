using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vurbiri
{
    [System.Serializable]
    public class LoadScene
    {
        [SerializeField] private int _scene;
        private AsyncOperation _asyncOperation = null;

        public AsyncOperation Start(bool allowSceneActivation = false)
        {
            if (_asyncOperation == null)
            {
                _asyncOperation = SceneManager.LoadSceneAsync(_scene);
                _asyncOperation.allowSceneActivation = allowSceneActivation;
            }

            return _asyncOperation;
        }

        public void End()
        {
            if (_asyncOperation == null)
                return;

            _asyncOperation.allowSceneActivation = true;
            _asyncOperation = null;
        }
    }
}
