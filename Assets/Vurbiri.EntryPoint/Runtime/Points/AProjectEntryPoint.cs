//Assets\Vurbiri.EntryPoint\Runtime\Points\AProjectEntryPoint.cs
using UnityEngine;

namespace Vurbiri.EntryPoint
{
    public abstract class AProjectEntryPoint : MonoBehaviour
    {
        private static AProjectEntryPoint s_instance;

        [SerializeField] private LoadScene _emptyScene;

        private AEnterParam _currentEnterParam;

        protected readonly DIContainer _projectContainer = new(null);
        protected Loading _loading;

        protected abstract ILoadingScreen Screen { get; }
        protected abstract string LoadingDesc { get; }

        private void Awake()
        {
            if (s_instance != null)
            {
                Destroy(gameObject); 
                return;
            }

            s_instance = this; 
            DontDestroyOnLoad(gameObject);
            gameObject.SetActive(true);

            _loading = Loading.Create(this, Screen);

            ASceneEntryPoint.EventLoaded += EnterScene;
        }

        private void LoadScene(ExitParam param)
        {
            _currentEnterParam = param.EnterParam;

            _loading.Add(_emptyScene.Load(), new LoadSceneStep(param.NextScene, LoadingDesc));
        }

        private void EnterScene(ASceneEntryPoint sceneEntryPoint)
        {
            sceneEntryPoint.Enter(new(_projectContainer), _loading, _currentEnterParam).Add(LoadScene);
        }
    }
}
