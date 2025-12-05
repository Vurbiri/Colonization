using System;
using System.Threading.Tasks;

namespace Vurbiri.EntryPoint
{
    sealed public class Transition
    {
        private static Transition s_instance;
        private static Action<int> s_onExit;

        private readonly VAction _eventExit = new();
        private readonly IEnterParam _enterParam;
        private ExitParam _exitParam;
        private IDisposable _sceneContainer;

        public static Event OnExit => s_instance._eventExit;

        internal static Transition Instance => s_instance;

        private Transition(IEnterParam enterParam) => _enterParam = enterParam;

        internal static void Create(Action<int> onExit, IEnterParam enterParam)
        {
            s_onExit = onExit;
            s_instance = new(enterParam);
        }

        public void Setup(IDisposable sceneContainer, int nextScene) => Setup(sceneContainer, new ExitParam(nextScene));
        public void Setup(IDisposable sceneContainer, ExitParam exitParam)
        {
            _exitParam = exitParam;
            _sceneContainer = sceneContainer;
        }

        public T GetEnterParam<T>() where T : IEnterParam => (T)_enterParam;

        public static void Exit() => s_instance.ExitInternal();
        public static void Exit(int nextScene) => Exit(new ExitParam(nextScene));
        public static void Exit(ExitParam exitParam)
        {
            s_instance._exitParam = exitParam;
            s_instance.ExitInternal();
        }

        private async void ExitInternal()
        {
            _eventExit.InvokeOneShot();
            await Task.Delay((int)(2000f * UnityEngine.Time.unscaledDeltaTime) + 1);
            _sceneContainer.Dispose();

            s_instance = new(_exitParam.EnterParam);
            s_onExit.Invoke(_exitParam.NextScene);
        }
    }
}
