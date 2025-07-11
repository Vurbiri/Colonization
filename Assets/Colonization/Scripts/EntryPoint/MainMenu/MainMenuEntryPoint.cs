using System.Collections;
using UnityEngine;
using Vurbiri.EntryPoint;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.EntryPoint
{
    public class MainMenuEntryPoint : ASceneEntryPoint
    {
        [SerializeField] private SceneId _nextScene;

        public override ISubscription<ExitParam> Enter(SceneContainer containers, Loading loading, AEnterParam param)
        {
            Debug.Log("MainMenu Enter");
            loading.Add(Exit_Cn());
            return new SceneExitPoint(_nextScene, containers).EventExit;
        }

        private IEnumerator Exit_Cn()
        {
            yield return new WaitForSecondsRealtime(.5f);
            SceneExitPoint.Exit();
            Debug.Log("MainMenu Exit");
        }
    }
}
