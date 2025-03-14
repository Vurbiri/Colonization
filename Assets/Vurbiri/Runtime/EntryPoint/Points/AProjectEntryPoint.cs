//Assets\Vurbiri\Runtime\EntryPoint\Points\AProjectEntryPoint.cs
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vurbiri.UI;

namespace Vurbiri.EntryPoint
{
    [DefaultExecutionOrder(5)]
    public class AProjectEntryPoint : AClosedSingleton<AProjectEntryPoint>
    {
        [SerializeField] private SceneId _emptyScene;

        protected DIContainer _servicesContainer = new(null);
        protected DIContainer _dataContainer = new(null);
        protected DIContainer _objectsContainer = new(null);

        protected LoadingScreen _loadingScreen;

        private AEnterParam _currentEnterParam;

        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);

            ASceneEntryPoint.EventLoaded += EnterScene;

            _loadingScreen = _objectsContainer.AddInstance(LoadingScreen.Create());
        }

        protected void LoadScene(ExitParam param)
        {
            _currentEnterParam = param.EnterParam;
            gameObject.SetActive(true);
            StartCoroutine(LoadScene_Cn(param.NextScene));
        }
        protected IEnumerator LoadScene_Cn(int sceneId)
        {
            _loadingScreen.TurnOnOf(true);

            yield return SceneManager.LoadSceneAsync(_emptyScene);
            SceneManager.LoadSceneAsync(sceneId);
        }

        protected void EnterScene(ASceneEntryPoint sceneEntryPoint)
        {
            sceneEntryPoint.Enter(new(_servicesContainer, _dataContainer, _objectsContainer), _currentEnterParam).Subscribe(LoadScene, false);
            gameObject.SetActive(false);
        }
    }
}
