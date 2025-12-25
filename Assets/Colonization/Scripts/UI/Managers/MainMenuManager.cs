using UnityEngine;
using Vurbiri.Colonization.UI;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
	public class MainMenuManager : MonoBehaviour
	{
        [SerializeField] private VButton _continue;
        [SerializeField] private WindowItem[] _windows;

        private void Start()
        {
            var manager = new Manager();
            for (int i = 0; i < Window.Count; ++i)
                _windows[i].Init(manager.OnOpenWindow, manager.OnCloseWindow);

            var game = ProjectContainer.GameSettings;
            if (_continue.interactable = game.IsLoad)
                _continue.AddListener(manager.Continue);

            Destroy(this);
        }

        #region Constants
        private readonly struct Window
        {
            public const int NewGame  = 0;
            public const int Settings = 1;
            public const int Help     = 2;

            public const int Count = 2;
        }
        #endregion

        private class Manager
        {
            private Switcher _current;

            public void OnOpenWindow(Switcher switcher)
            {
                _current?.Close();
                _current = switcher;
            }
            public void OnCloseWindow(Switcher switcher)
            {
                if (_current == switcher)
                    _current = null;
            }

            public void Continue()
            {
                _current?.Close();
                Vurbiri.EntryPoint.Transition.Exit();
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying) return;

            EUtility.SetObject(ref _continue, "ContinueButton");

            EUtility.SetArray(ref _windows, Window.Count);
            _windows[Window.NewGame]?.OnValidate("New");
            _windows[Window.Settings]?.OnValidate("Settings");
        }

        public void SetColors_Ed(SceneColorsEd colors)
        {
            foreach (var image in GetComponentsInChildren<UnityEngine.UI.Image>())
                image.SetColorField(colors.menu);

            foreach (var text in GetComponentsInChildren<TMPro.TMP_Text>())
                text.SetColorField(colors.panelText);
        }
#endif
    }
}
