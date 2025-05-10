//Assets\Vurbiri.EntryPoint\Runtime\Points\ASceneExitPoint.cs
using Vurbiri.Reactive;

namespace Vurbiri.EntryPoint
{
    public abstract class ASceneExitPoint
    {
        private static ASceneExitPoint _instance;

        private readonly SceneContainer _sceneContainer;
        private readonly Signer<ExitParam> _eventExit = new();

        protected ExitParam _exitParam;

        public ISigner<ExitParam> EventExit => _eventExit;

        public ASceneExitPoint(ExitParam exitParam, SceneContainer sceneContainer)
        {
            _exitParam = exitParam;
            _sceneContainer = sceneContainer;
            _instance = this;
        }

        public static void Exit() => _instance.OnExit();

        protected virtual void OnExit()
        {
            _sceneContainer.Dispose();
            _eventExit.Invoke(_exitParam);

            _instance = null;
        }
    }
}
