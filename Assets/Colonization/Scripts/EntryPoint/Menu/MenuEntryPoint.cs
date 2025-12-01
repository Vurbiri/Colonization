using System.Collections;
using UnityEngine;
using Vurbiri.EntryPoint;
using Vurbiri.International;

namespace Vurbiri.Colonization.EntryPoint
{
    public class MenuEntryPoint : ASceneEntryPoint
    {
        [SerializeField] private SceneId _nextScene;
        [SerializeField] private FileIds _localizationFiles = new(false);

        public override void Enter(Loading loading, Transition transition)
        {
            transition.Setup(new MenuContainer(new()), _nextScene);
            print("MainMenu Enter");
            loading.Add(Exit_Cn());
        }

        private IEnumerator Exit_Cn()
        {
            Localization.Instance.SetFiles(_localizationFiles);
            yield return new WaitRealtime(.5f);
            Transition.Exit();
            print("MainMenu Exit");
        }
    }
}
