using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vurbiri.Localization;
using Vurbiri.UI;

namespace Vurbiri.EntryPoint
{
    public class AProjectEntryPoint : AClosedSingleton<AProjectEntryPoint>
    {
        [SerializeField] private SceneId _emptyScene;

        protected DIContainer _servicesContainer = new(null);
        protected DIContainer _dataContainer = new(null);
        protected DIContainer _objectsContainer = new(null);

        protected LoadingScreen _loadingScreen;

        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);

            ASceneEntryPoint.EventLoad += EnterScene;

            _servicesContainer.AddInstance(Coroutines.Create("ProjectCoroutines", true));
            _servicesContainer.AddInstance(Language.Create());

            _loadingScreen = _objectsContainer.AddInstance(LoadingScreen.Create());
        }

        protected IEnumerator LoadScene_Coroutine(int sceneId)
        {
            _loadingScreen.TurnOnOf(true);

            yield return SceneManager.LoadSceneAsync(_emptyScene);
            SceneManager.LoadSceneAsync(sceneId);
        }

        protected void EnterScene(ASceneEntryPoint sceneEntryPoint)
        {
            sceneEntryPoint.Enter(new(_servicesContainer, _dataContainer, _objectsContainer));
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_loadingScreen == null)
                _loadingScreen = VurbiriEditor.Utility.FindAnyPrefab<LoadingScreen>();
        }
#endif
    }
}
