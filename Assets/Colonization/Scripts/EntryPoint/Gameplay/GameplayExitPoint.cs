//Assets\Colonization\Scripts\EntryPoint\Gameplay\GameplayExitPoint.cs
using System;
using System.Collections;
using Vurbiri.EntryPoint;
using Vurbiri.UI;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed public class GameplayExitPoint : ASceneExitPoint
    {
        private readonly ExitParam _exitParam;

        public GameplayExitPoint(int nextScene, SceneContainer sceneContainers) : base(sceneContainers)
        {
            _exitParam = new(nextScene);
        }

        sealed protected override void OnExit(Action<ExitParam> callback)
        {
            SceneContainer.Get<Coroutines>().Run(OnExit_Cn(callback));
        }

        private IEnumerator OnExit_Cn(Action<ExitParam> callback)
        {
            yield return SceneContainer.Get<LoadingScreen>().SmoothOn_Wait();
            callback(_exitParam);
        }
    }
}
