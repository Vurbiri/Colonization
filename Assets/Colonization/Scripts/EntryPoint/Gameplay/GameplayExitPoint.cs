using System;
using System.Collections;
using Vurbiri.EntryPoint;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
    public class GameplayExitPoint : ASceneExitPoint
    {
        private readonly ExitParam _exitParam;

        public GameplayExitPoint(int nextScene) : base(new())
        {
            _exitParam = new(nextScene);
        }

        protected override void OnExit(Action<ExitParam> callback)
        {
            SceneServices.Get<Coroutines>().Run(OnExit_Coroutine(callback));
        }

        private IEnumerator OnExit_Coroutine(Action<ExitParam> callback)
        {
            yield return SceneObjects.Get<LoadingScreen>().SmoothOn_Wait();
            callback(_exitParam);
        }
    }
}
