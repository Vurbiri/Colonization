//Assets\Vurbiri\Runtime\EntryPoint\Points\ASceneExitPoint.cs
using System;
using Vurbiri.Reactive;

namespace Vurbiri.EntryPoint
{
    public abstract class ASceneExitPoint
    {
        private static ASceneExitPoint _instance;

        private readonly SceneContainer _sceneContainer;
        private readonly Subscriber<ExitParam> _exit = new();
        
        public ISubscriber<ExitParam> EventExit => _exit;

        public ASceneExitPoint(SceneContainer sceneContainer)
        {
            _sceneContainer = sceneContainer;
            _instance = this;
        }

        public static void Exit() => _instance.OnExit(_instance.ExitCallback);

        protected abstract void OnExit(Action<ExitParam> callback);

        private void ExitCallback(ExitParam param)
        {
            _sceneContainer.Dispose();
            _exit.Invoke(param);

            _instance = null;
            _exit.Dispose();
        }
    }
}
