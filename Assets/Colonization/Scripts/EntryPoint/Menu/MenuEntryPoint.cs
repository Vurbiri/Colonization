using System.Collections;
using Vurbiri.EntryPoint;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.EntryPoint
{
    public class MenuEntryPoint : ASceneEntryPoint
    {
        [UnityEngine.SerializeField] private SceneId _nextScene;

        public override ISubscription<ExitParam> Enter(Loading loading, AEnterParam param)
        {
            var container = new MenuContainer(new());
            print("MainMenu Enter");
            loading.Add(Exit_Cn());
            return new SceneExitPoint(_nextScene, container).EventExit;
        }

        private IEnumerator Exit_Cn()
        {
            yield return new WaitRealtime(.5f);
            SceneExitPoint.Exit();
            print("MainMenu Exit");
        }
    }
}
