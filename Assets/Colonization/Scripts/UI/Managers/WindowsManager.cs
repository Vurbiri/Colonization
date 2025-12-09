using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    [System.Serializable]
    public class WindowsManager
    {
        [SerializeField] private PerksWindow _perksWindow;
        [SerializeField] private HintButton _perksButton;
        [Space]
        [SerializeField] private ExchangeWindow _exchangeWindow;
        [SerializeField] private HintButton _exchangeButton;
        [Space]
        [SerializeField] private GiftWindow _giftWindow;
        [SerializeField] private GiftButton[] _giftButtons;
        [Space]
        [SerializeField] private GameSettingsWindow _settingsWindow;
        [SerializeField] private HintButton _settingsButton;
        [Space]
        [SerializeField] private HelpWindow _helpWindow;
        [SerializeField] private HintButton _helpButton;
        [Space]
        [SerializeField] private DiceWindow _diceWindow;
        [Space]
        [SerializeField] private GameOverWindow _gameOverWindow;

        private readonly Switcher[] _windows = new Switcher[Window.Count];
        private readonly HintButton[] _gameButtons = new HintButton[GameButton.Count];
        private readonly HintButton[] _utilityButtons = new HintButton[UtilityButton.Count];
        private int _openWindowsCount;

        public void Init()
        {
            _windows[Window.Perks]    = _perksWindow.Init().Setup(Window.Perks, OnOpenWindow, OnCloseWindow);
            _windows[Window.Exchange] = _exchangeWindow.Init().Setup(Window.Exchange, OnOpenWindow, OnCloseWindow);
            _windows[Window.Gift]     = _giftWindow.Init(_giftButtons).Setup(Window.Gift, OnOpenWindow, OnCloseWindow);
            _windows[Window.Settings] = _settingsWindow.Init().Setup(Window.Settings, OnOpenWindow, OnCloseWindow);
            _windows[Window.Help]     = _helpWindow.Init().Setup(Window.Help, OnOpenWindow, OnCloseWindow);

            _diceWindow.Init(Window.Dice, OnOpenWindow, OnCloseWindow);
            _gameOverWindow.Init(Window.GameOver, OnOpenWindow);

            _gameButtons[GameButton.Perks]    = _perksButton; 
            _gameButtons[GameButton.Exchange] = _exchangeButton;

            _utilityButtons[UtilityButton.Settings] = _settingsButton;
            _utilityButtons[UtilityButton.Help]     = _helpButton;

            _perksButton.AddListener(_windows[Window.Perks].Switch);
            _exchangeButton.AddListener(_windows[Window.Exchange].Switch);
            _settingsButton.AddListener(_windows[Window.Settings].Switch);
            _helpButton.AddListener(_windows[Window.Help].Switch);

            var game = GameContainer.GameEvents;
            game.Subscribe(GameModeId.WaitRoll, (_, _) => SetUtilityButtons(false));
            game.Subscribe(GameModeId.Profit, (_, _) => SetUtilityButtons(true));
            game.Subscribe(GameModeId.GameOver, (_, _) => SetUtilityButtons(false));

            GameContainer.Person.Interactable.Subscribe(OnInteractable);

            _perksButton = null; _exchangeButton = null; _settingsButton = null; _helpButton = null;
            _perksWindow = null; _exchangeWindow = null; _settingsWindow = null; _helpWindow = null; _giftWindow = null; _diceWindow = null; _gameOverWindow = null;
        }

        private void OnInteractable(bool interactable)
        {
            for (int i = 0; i < GameButton.Count; i++)
                _gameButtons[i].Unlock = interactable;

            for (int i = _giftButtons.Length - 1; i >= 0; i--)
                _giftButtons[i].interactable = interactable;

            if (!interactable)
            {
                for (int i = 0; i < Window.Count; i++)
                    _windows[i].Close();
            }
        }

        private void OnOpenWindow(int id)
        {
            if (_openWindowsCount++ == 0)
            {
                GameContainer.InputController.Unselect();
                GameContainer.InputController.WindowMode(true);
            }

            for (int i = 0; i < Window.Count; i++)
                _windows[i].TryClose(id);
        }
        private void OnCloseWindow()
        {
            if (--_openWindowsCount == 0)
            {
                GameContainer.InputController.WindowMode(false);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetUtilityButtons(bool unlock)
        {
            for (int i = 0; i < UtilityButton.Count; i++)
                _utilityButtons[i].Unlock = unlock;
        }

        #region Constants
        // ------------------------------------------
        private readonly struct Window
        {
            public const int GameOver = -2;
            public const int Dice     = -1;
            public const int Perks    =  0;
            public const int Exchange =  1;
            public const int Gift     =  2;
            public const int Settings =  3;
            public const int Help     =  4;

            public const int Count = 5;
        }
        // ------------------------------------------
        private readonly struct GameButton
        {
            public const int Perks    = 0;
            public const int Exchange = 1;

            public const int Count = 2;
        }
        // ------------------------------------------
        private readonly struct UtilityButton
        {
            public const int Settings = 0;
            public const int Help     = 1;

            public const int Count = 2;
        }
        #endregion

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
            EUtility.SetObject(ref _settingsButton, "SettingsButton");

            EUtility.SetObject(ref _helpWindow);
            EUtility.SetObject(ref _helpButton, "HelpButton");

            EUtility.SetObject(ref _diceWindow);

            EUtility.SetObject(ref _gameOverWindow);
        }
#endif
    }
}
