using UnityEngine;
using Vurbiri.Colonization.UI;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
	public class MainMenuManager : MonoBehaviour
	{
        [SerializeField] private VButton _continue;
        [SerializeField] private WindowItem[] _windows;

        private Switcher _current;

        private void Start()
        {
			var game = ProjectContainer.GameSettings;

			if (_continue.interactable = game.IsLoad)
                _continue.AddListener(Vurbiri.EntryPoint.Transition.Exit);


            for(int i = 0; i < Window.Count; ++i)
                _windows[i].Init(OnOpenWindow, OnCloseWindow);

            Destroy(this);
        }

        private void OnOpenWindow(Switcher switcher)
        {
            _current?.SilentClose();
            _current = switcher;
        }
        private void OnCloseWindow(Switcher switcher)
        {
            if (_current == switcher)
                _current = null;
        }

        #region Constants
        private readonly struct Window
        {
            public const int NewGame  = 0;
            public const int Settings = 1;
            public const int Help     = 2;

            public const int Count = 1;
        }
        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying) return;

            EUtility.SetObject(ref _continue, "ContinueButton");

            EUtility.SetArray(ref _windows, Window.Count);
            _windows[0]?.OnValidate("Settings");
            //_windows[Window.Exchange]?.OnValidate("ExchangeWindow", "ExchangeButton");
        }
#endif
    }
}
