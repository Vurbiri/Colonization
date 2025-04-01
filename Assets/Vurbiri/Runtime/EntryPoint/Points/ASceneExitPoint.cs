//Assets\Vurbiri\Runtime\EntryPoint\Points\ASceneExitPoint.cs
using System;
using Vurbiri.Reactive;

namespace Vurbiri.EntryPoint
{
    public abstract class ASceneExitPoint
    {
        private static ASceneExitPoint _instance;

        private readonly SceneContainer _sceneContainer;
        private readonly Subscriber<ExitParam> _eventExit = new();
        
        public ISubscriber<ExitParam> EventExit => _eventExit;

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
            _eventExit.Invoke(param);

            _instance = null;
        }
    }
}
