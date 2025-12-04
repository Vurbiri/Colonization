using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    [System.Serializable]
    public class WindowsManager
    {
        private const int GAME_WINDOWS_COUNT = 4, BUTTONS_COUNT = 2;
        
        [SerializeField] private PerksWindow _perksWindow;
        [SerializeField] private HintButton _perksButton;
        [Space]
        [SerializeField] private ExchangeWindow _exchangeWindow;
        [SerializeField] private HintButton _exchangeButton;
        [Space]
        [SerializeField] private GiftWindow _giftWindow;
        [SerializeField] private GiftButton[] _giftButtons;
        [Space]
        [SerializeField] private SettingsWindow _settingsWindow;
        [Space]
        [SerializeField] private DiceWindow _diceWindow;
        [Space]
        [SerializeField] private GameOverWindow _gameOverWindow;

        private readonly Switcher[] _gameWindows = new Switcher[GAME_WINDOWS_COUNT];
        private readonly HintButton[] _buttons = new HintButton[BUTTONS_COUNT];
        private int _openWindowsCount;

        public void Init()
        {
            _buttons[0] = _perksButton; _buttons[1] = _exchangeButton;

            int id = 0;
            _gameWindows[id] = _perksWindow.Init(_perksButton).Setup(id++, OnOpenWindow, OnCloseWindow);
            _gameWindows[id] = _exchangeWindow.Init(_exchangeButton).Setup(id++, OnOpenWindow, OnCloseWindow);
            _gameWindows[id] = _giftWindow.Init(_giftButtons).Setup(id++, OnOpenWindow, OnCloseWindow);
            _gameWindows[id] = _settingsWindow.Switcher.Setup(id++, OnOpenWindow, OnCloseWindow);

            id = 0;
            _diceWindow.Init(--id, OnOpenWindow, OnCloseWindow);
            _gameOverWindow.Init(--id, OnOpenWindow);

            GameContainer.Person.Interactable.Subscribe(OnInteractable);

            _perksWindow = null; _exchangeWindow = null; _giftWindow = null; _diceWindow = null; _gameOverWindow = null;
            _perksButton = null; _exchangeButton = null; 
        }

        private void OnInteractable(bool interactable)
        {
            for (int i = 0; i < BUTTONS_COUNT; i++)
                _buttons[i].Unlock = interactable;

            for (int i = _giftButtons.Length - 1; i >= 0; i--)
                _giftButtons[i].interactable = interactable;

            if (!interactable)
            {
                for (int i = 0; i < GAME_WINDOWS_COUNT; i++)
                    _gameWindows[i].Close();
            }
        }

        private void OnOpenWindow(int id)
        {
            if (_openWindowsCount++ == 0)
            {
                GameContainer.InputController.Unselect();
                GameContainer.InputController.WindowMode(true);
            }

            for (int i = 0; i < GAME_WINDOWS_COUNT; i++)
                _gameWindows[i].TryClose(id);
        }
        private void OnCloseWindow()
        {
            if (--_openWindowsCount == 0)
            {
                GameContainer.InputController.WindowMode(false);
            }
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            if (Application.isPlaying) return;
            
            EUtility.SetObject(ref _perksWindow);
            EUtility.SetObject(ref _perksButton, "PerksButton");

            EUtility.SetObject(ref _exchangeWindow);
            EUtility.SetObject(ref _exchangeButton, "ExchangeButton");

            EUtility.SetObject(ref _giftWindow);
            EUtility.SetObjects(ref _giftButtons);

            EUtility.SetObject(ref _settingsWindow);

            EUtility.SetObject(ref _diceWindow);

            EUtility.SetObject(ref _gameOverWindow);
        }
#endif
    }
}
