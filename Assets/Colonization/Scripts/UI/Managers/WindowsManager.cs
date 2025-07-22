using UnityEngine;
using Vurbiri.Colonization.EntryPoint;

namespace Vurbiri.Colonization.UI
{
    [System.Serializable]
    public class WindowsManager
	{
        private const int COUNT = 2;
        
        [SerializeField] private PerksWindow _perksWindow;
        [SerializeField] private ExchangeWindow _exchangeWindow;
        [Space]
        [SerializeField, ReadOnly] private HintButton[] _buttons;

        private IWindow[] _windows;
        private int _openWindowsCount;

        public void Init(GameContent content)
        {
            _windows = new IWindow[] { _perksWindow, _exchangeWindow };

            var player = content.players.Person;

            Debug.Log("WindowsManager - убрать комментарии - /*, false*/");

            IWindow window;
            for (int i = 0; i < COUNT; i++)
            {
                window = _windows[i];
                window.Init(player, content.canvasHint, false);
                window.OnOpen.Add(OnOpenWindow);
                window.OnClose.Add(OnCloseWindow);

                _buttons[i].Init(content.canvasHint, window.Switch/*, false*/);
            }

            _perksWindow.OnOpen.Add(_exchangeWindow.Close);
            _exchangeWindow.OnOpen.Add(_perksWindow.Close);

            player.Interactable.Subscribe(OnInteractable);

            _perksWindow = null; _exchangeWindow = null;
        }

        private void OnInteractable(bool interactable)
        {
            if (interactable)
            {
                for (int i = 0; i < COUNT; i++)
                    _buttons[i].Interactable = true;
            }
            else
            {
                for (int i = 0; i < COUNT; i++)
                {
                    _buttons[i].Interactable = false;
                    _windows[i].Close();
                }
            }
        }

        private void OnOpenWindow()
        {
            if (_openWindowsCount++ == 0)
            {
                GameContainer.InputController.Unselect();
                GameContainer.InputController.UIMode(true);
            }
        }
        private void OnCloseWindow()
        {
            if (--_openWindowsCount == 0)
                GameContainer.InputController.UIMode(false);
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            if (Application.isPlaying) return;
            
            EUtility.SetObject(ref _perksWindow);
            EUtility.SetObject(ref _exchangeWindow);

            EUtility.SetArray(ref _buttons, COUNT);
            EUtility.SetObject(ref _buttons[0], "PerksButton");
            EUtility.SetObject(ref _buttons[1], "ExchangeButton");
        }
#endif
    }
}
