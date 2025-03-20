//Assets\Vurbiri\Runtime\EntryPoint\Points\AProjectEntryPoint.cs
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vurbiri.UI;

namespace Vurbiri.EntryPoint
{
    public class AProjectEntryPoint : MonoBehaviour
    {
        [SerializeField] private SceneId _emptyScene;

        private static AProjectEntryPoint _instance;

        private AEnterParam _currentEnterParam;

        protected DIContainer _servicesContainer = new(null);
        protected DIContainer _dataContainer = new(null);
        protected DIContainer _objectsContainer = new(null);

        protected LoadingScreen _loadingScreen;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            ASceneEntryPoint.EventLoaded += EnterScene;
            _objectsContainer.AddInstance(_loadingScreen = LoadingScreen.Create());
        }

        protected void LoadScene(ExitParam param)
        {
            _currentEnterParam = param.EnterParam;

            gameObject.SetActive(true);
            StartCoroutine(LoadScene_Cn(param.NextScene));

            #region Local: LoadScene_Cn(..)
            //=================================
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
            sceneEntryPoint.Enter(new(_servicesContainer, _dataContainer, _objectsContainer), _currentEnterParam).Add(LoadScene);
            gameObject.SetActive(false);
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }
    }
}
