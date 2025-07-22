using System.Collections;
using UnityEngine;
using Vurbiri.EntryPoint;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.EntryPoint
{
    public class MainMenuEntryPoint : ASceneEntryPoint
    {
        [SerializeField] private SceneId _nextScene;

        public override ISubscription<ExitParam> Enter(Loading loading, AEnterParam param)
        {
            var container = new MainMenuContainer(new());
            Debug.Log("MainMenu Enter");
            Debug.Log(MainMenuContainer.Settings.Profile.Quality);
            loading.Add(Exit_Cn());
            return new SceneExitPoint(_nextScene, container).EventExit;
        }

        private IEnumerator Exit_Cn()
        {
            yield return new WaitForSecondsRealtime(.5f);
            SceneExitPoint.Exit();
            Debug.Log("MainMenu Exit");
        }
    }
}
