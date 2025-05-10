//Assets\Vurbiri.EntryPoint\Runtime\Points\AProjectEntryPoint.cs
using UnityEngine;

namespace Vurbiri.EntryPoint
{
    public abstract class AProjectEntryPoint : MonoBehaviour
    {
        private static AProjectEntryPoint _instance;

        [SerializeField] private LoadScene _emptyScene;
        private readonly PostLoadSceneStep _postLoad = new();

        private AEnterParam _currentEnterParam;

        protected readonly DIContainer _projectContainer = new(null);
        protected Loading _loading;
        protected ILoadingScreen _loadingScreen;

        protected abstract ILoadingScreen Screen { get; }
        protected abstract string LoadingDesc { get; }

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject); 
                return;
            }

            _instance = this; 
            DontDestroyOnLoad(gameObject);
            gameObject.SetActive(true);

            _loading = Loading.Create(this, Screen);

            ASceneEntryPoint.EventLoaded += EnterScene;
        }

        private void LoadScene(ExitParam param)
        {
            _currentEnterParam = param.EnterParam;

            _loading.Add(_emptyScene, new LoadSceneStep(param.NextScene, LoadingDesc), _postLoad.Restart(LoadingDesc));
        }

        private void EnterScene(ASceneEntryPoint sceneEntryPoint)
        {
            sceneEntryPoint.Enter(new(_projectContainer), _loading, _currentEnterParam).Add(LoadScene);
            _postLoad.Stop();
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }
    }
}
