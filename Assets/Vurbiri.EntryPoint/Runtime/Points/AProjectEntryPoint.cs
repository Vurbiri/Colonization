using System;
using UnityEngine;

namespace Vurbiri.EntryPoint
{
    public abstract class AProjectEntryPoint : MonoBehaviour
    {
        protected static AProjectEntryPoint s_instance;

        [SerializeField] private LoadScene _emptyScene;

        private IDisposable _container;
        protected Loading _loading;

        protected abstract string LoadingDesc { get; }

        private void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected void Init(IDisposable container, ILoadingScreen screen, IEnterParam enterParam = null)
        {
            _container = container;
            _loading = Loading.Create(screen, this);
            Transition.Create(LoadScene, enterParam);
            ASceneEntryPoint.EventLoaded.Add(EnterScene);
        }

        private void LoadScene(int nextScene)
        {
            _loading.Add(_emptyScene.Load(), new LoadSceneStep(nextScene, LoadingDesc));
        }

        private void EnterScene(ASceneEntryPoint sceneEntryPoint)
        {
            sceneEntryPoint.Enter(_loading, Transition.Instance);
        }

        private void OnDestroy()
        {
            if (s_instance == this)
            {
                _container.Dispose();
                s_instance = null;
            }
        }
    }
}
