using System;
using Vurbiri.Reactive;

namespace Vurbiri.EntryPoint
{
    sealed public class Transition
    {
        private static Transition s_instance;
        private static Action<int> s_onExit;

        private readonly Subscription _eventExit = new();
        private readonly IEnterParam _enterParam;
        private ExitParam _exitParam;
        private IContainer _sceneContainer;

        public static Event OnExit => s_instance._eventExit;

        internal static Transition Instance => s_instance;

        private Transition(IEnterParam enterParam) => _enterParam = enterParam;

        internal static void Create(Action<int> onExit, IEnterParam enterParam)
        {
            s_onExit = onExit;
            s_instance = new(enterParam);
        }

        public void Setup(int nextScene, IContainer sceneContainer) => Setup(new ExitParam(nextScene), sceneContainer);
        public void Setup(ExitParam exitParam, IContainer sceneContainer)
        {
            _exitParam = exitParam;
            _sceneContainer = sceneContainer;
        }

        public T GetEnterParam<T>() where T : IEnterParam => (T)_enterParam;

        public static void Exit() => s_instance.InternalExit();
        public static void Exit(int nextScene) => Exit(new ExitParam(nextScene));
        public static void Exit(ExitParam exitParam)
        {
            s_instance._exitParam = exitParam;
            s_instance.InternalExit();
        }

        private void InternalExit()
        {
            _eventExit.InvokeOneShot();
            _sceneContainer.Dispose();

            s_instance = new(_exitParam.EnterParam);
            s_onExit.Invoke(_exitParam.NextScene);
        }
    }
}
