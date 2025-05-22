//Assets\Vurbiri.EntryPoint\Runtime\Points\ASceneExitPoint.cs
using Vurbiri.Reactive;

namespace Vurbiri.EntryPoint
{
    public abstract class ASceneExitPoint
    {
        private static ASceneExitPoint s_instance;

        private readonly SceneContainer _sceneContainer;
        private readonly Subscription<ExitParam> _eventExit = new();

        protected ExitParam _exitParam;

        public ISubscription<ExitParam> EventExit => _eventExit;

        public ASceneExitPoint(ExitParam exitParam, SceneContainer sceneContainer)
        {
            _exitParam = exitParam;
            _sceneContainer = sceneContainer;
            s_instance = this;
        }

        public static void Exit() => s_instance.OnExit();

        protected virtual void OnExit()
        {
            _sceneContainer.Dispose();
            _eventExit.Invoke(_exitParam);

            s_instance = null;
        }
    }
}
