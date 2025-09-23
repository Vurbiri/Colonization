using System.Collections;
using Vurbiri.EntryPoint;

namespace Vurbiri.Colonization.EntryPoint
{
    public class MenuEntryPoint : ASceneEntryPoint
    {
        [UnityEngine.SerializeField] private SceneId _nextScene;

        public override void Enter(Loading loading, Transition transition)
        {
            transition.Setup(_nextScene, new MenuContainer(new()));
            print("MainMenu Enter");
            loading.Add(Exit_Cn());
        }

        private IEnumerator Exit_Cn()
        {
            yield return new WaitRealtime(.5f);
            Transition.Exit();
            print("MainMenu Exit");
        }
    }
}
