using System;
using Vurbiri.Reactive;

namespace Vurbiri.EntryPoint
{
    public abstract class ASceneExitPoint
    {
        private static ASceneExitPoint s_instance;

        private readonly IDisposable _sceneContainer;
        private readonly Subscription<ExitParam> _eventExit = new();

        protected ExitParam _exitParam;

        public ISubscription<ExitParam> EventExit => _eventExit;

        public ASceneExitPoint(ExitParam exitParam, IDisposable sceneContainer)
        {
            _exitParam = exitParam;
            _sceneContainer = sceneContainer;
            s_instance = this;
        }

        public static void Exit() => s_instance.OnExit();
        public static void Exit(ExitParam exitParam)
        {
            s_instance._exitParam = exitParam;
            s_instance.OnExit();
        }

        protected virtual void OnExit()
        {
            s_instance = null;

            _sceneContainer.Dispose();
            _eventExit.Invoke(_exitParam);
        }
    }
}
