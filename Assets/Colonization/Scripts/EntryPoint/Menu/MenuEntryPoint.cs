using System.Collections;
using UnityEngine;
using Vurbiri.EntryPoint;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization.EntryPoint
{
    public class MenuEntryPoint : ASceneEntryPoint
    {
        [SerializeField] private SceneId _nextScene;
        [SerializeField] private FileIds _localizationFiles = new(false);
        [Space]
        [SerializeField] private MessageBoxColors _messageBoxColors;

        public override void Enter(Loading loading, Transition transition)
        {
            Log.Info("[MainMenuPoint] Enter");

            transition.Setup(new MenuContainer(new()), _nextScene);
            
            Localization.Instance.SetFiles(_localizationFiles, true);
            MessageBox.SetColors(_messageBoxColors);

            //loading.Add(Exit_Cn());
        }

        private IEnumerator Exit_Cn()
        {
            yield return new WaitRealtime(.5f);
            Transition.Exit();
            print("[MainMenu] Exit");
        }

        private class Dummy : System.IDisposable { public void Dispose() { } }


#if UNITY_EDITOR
        public void SetColors_Ed(SceneColorsEd colors)
        {
            _messageBoxColors.window = colors.panelBack;
            _messageBoxColors.text = colors.panelText;
            _messageBoxColors.button = colors.elements;
        }
#endif
    }
}
