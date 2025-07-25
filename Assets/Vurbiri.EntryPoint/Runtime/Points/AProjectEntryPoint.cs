using System;
using UnityEngine;

namespace Vurbiri.EntryPoint
{
    public abstract class AProjectEntryPoint : AClosedSingleton<AProjectEntryPoint>
    {
        [SerializeField] private LoadScene _emptyScene;

        private AEnterParam _currentEnterParam;
        private IDisposable _container;
        protected Loading _loading;

        protected abstract string LoadingDesc { get; }

        protected void Init(IDisposable container, ILoadingScreen screen)
        {
            _container = container;
            _loading = Loading.Create(this, screen);
            ASceneEntryPoint.EventLoaded.Add(EnterScene);
        }

        private void LoadScene(ExitParam param)
        {
            _currentEnterParam = param.EnterParam;
            _loading.Add(_emptyScene.Load(), new LoadSceneStep(param.NextScene, LoadingDesc));
        }

        private void EnterScene(ASceneEntryPoint sceneEntryPoint)
        {
            sceneEntryPoint.Enter(_loading, _currentEnterParam).Add(LoadScene);
        }

        protected override void OnDestroy()
        {
            if (s_instance == this)
                _container.Dispose();

            base.OnDestroy();
        }
    }
}
