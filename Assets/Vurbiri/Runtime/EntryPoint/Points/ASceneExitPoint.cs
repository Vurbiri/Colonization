//Assets\Vurbiri\Runtime\EntryPoint\Points\ASceneExitPoint.cs
using System;
using Vurbiri.Reactive;

namespace Vurbiri.EntryPoint
{
    public abstract class ASceneExitPoint
    {
        private static ASceneExitPoint _instance;

        protected readonly ReactiveValue<ExitParam> _exitEvent;

        public IReactive<ExitParam> ExitParam => _exitEvent;

        public ASceneExitPoint(ReactiveValue<ExitParam> exitEvent)
        {
            _exitEvent = exitEvent;
            _instance = this;
        }

        public static void Exit() => _instance.OnExit(_instance.ExitCallback);

        protected abstract void OnExit(Action<ExitParam> callback);

        private void ExitCallback(ExitParam param)
        {
            _instance = null;
            _exitEvent.Value = param;
        }
    }
}
