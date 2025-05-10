//Assets\Colonization\Scripts\EntryPoint\Menu\MenuEntryPoint.cs
using System.Collections;
using UnityEngine;
using Vurbiri.EntryPoint;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.EntryPoint
{
    public class MainMenuEntryPoint : ASceneEntryPoint
    {
        [SerializeField] private SceneId _nextScene;

        public override ISigner<ExitParam> Enter(SceneContainer containers, Loading loading, AEnterParam param)
        {
            Debug.Log("MainMenu Enter");
            StartCoroutine(Exit_Cn());
            return new SceneExitPoint(_nextScene, containers).EventExit;
        }

        private IEnumerator Exit_Cn()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            SceneExitPoint.Exit();
            Debug.Log("MainMenu Exit");
        }
    }
}
