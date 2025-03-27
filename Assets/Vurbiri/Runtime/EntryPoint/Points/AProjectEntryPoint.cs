//Assets\Vurbiri\Runtime\EntryPoint\Points\AProjectEntryPoint.cs
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vurbiri.UI;

namespace Vurbiri.EntryPoint
{
    public class AProjectEntryPoint : MonoBehaviour
    {
        private static AProjectEntryPoint _instance;

        [SerializeField] private SceneId _emptyScene;
        
        private AEnterParam _currentEnterParam;

        protected readonly DIContainer _projectContainer = new(null);
        protected LoadingScreen _loadingScreen;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject); return;
            }
            _instance = this; DontDestroyOnLoad(gameObject);

            ASceneEntryPoint.EventLoaded += EnterScene;
            _projectContainer.AddInstance(_loadingScreen = LoadingScreen.Create());
        }

        private void LoadScene(ExitParam param)
        {
            _currentEnterParam = param.EnterParam;

            gameObject.SetActive(true);
            StartCoroutine(LoadScene_Cn(param.NextScene));

            #region ===== Local Coroutine =====
            IEnumerator LoadScene_Cn(int sceneId)
            {
                _loadingScreen.TurnOnOf(true);

                yield return SceneManager.LoadSceneAsync(_emptyScene);
                SceneManager.LoadSceneAsync(sceneId);
            }
            #endregion
        }

        private void EnterScene(ASceneEntryPoint sceneEntryPoint)
        {
            sceneEntryPoint.Enter(new(_projectContainer), _currentEnterParam).Add(LoadScene);
            gameObject.SetActive(false);
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }
    }
}
