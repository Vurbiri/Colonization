using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    [System.Serializable]
    public class WindowsManager
    {
        [SerializeField] private WindowItem[] _windows;
        [Space]
        [SerializeField] private GiftWindow _giftWindow;
        [SerializeField] private GiftButton[] _giftButtons;
        [Space]
        [SerializeField] private DiceWindow _diceWindow;
        [Space]
        [SerializeField] private GameOverWindow _gameOverWindow;

        private readonly AVButton[] _gameButtons = new AVButton[GameButton.Count];
        private readonly AVButton[] _utilityButtons = new AVButton[UtilityButton.Count];
        private Switcher _current;

        public void Init()
        {
            _windows[Window.Perks].Init(OnOpenWindow, OnCloseWindow, ref _gameButtons[GameButton.Perks]);
            _windows[Window.Exchange].Init(OnOpenWindow, OnCloseWindow, ref _gameButtons[GameButton.Exchange]);
            _windows[Window.Settings].Init(OnOpenWindow, OnCloseWindow, ref _utilityButtons[UtilityButton.Settings]);
            _windows[Window.Help].Init(OnOpenWindow, OnCloseWindow, ref _utilityButtons[UtilityButton.Help]);

            _giftWindow.Init(_giftButtons).Setup(OnOpenWindow, OnCloseWindow);
            _diceWindow.Init().Setup(OnOpenWindow, OnCloseWindow);
            
            _gameOverWindow.Init(OnOpenWindow);

            GameContainer.GameEvents.Subscribe(SetUtilityButtons);
            GameContainer.Person.Interactable.Subscribe(OnInteractable);

            _windows = null; _giftWindow = null; _diceWindow = null; _gameOverWindow = null;
        }

        private void OnInteractable(bool interactable)
        {
            for (int i = 0; i < GameButton.Count; i++)
                _gameButtons[i].Unlock = interactable;

            for (int i = _giftButtons.Length - 1; i >= 0; i--)
                _giftButtons[i].interactable = interactable;

            if (!interactable && _current != null)
                _current.ForceClose();
        }

        private void OnOpenWindow(Switcher switcher)
        {
            if (_current == null)
            {
                GameContainer.InputController.Unselect();
                GameContainer.InputController.WindowMode(true);
            }
            else
            {
                _current.SilentClose();
            }

            _current = switcher;
        }
        private void OnCloseWindow(Switcher switcher)
        {
            if (_current == switcher)
            {
                _current = null;
                GameContainer.InputController.WindowMode(false);
            }
        }

        private void SetUtilityButtons(Id<GameModeId> gameMode, TurnQueue turn)
        {
            bool unlock = gameMode != GameModeId.WaitRoll & gameMode != GameModeId.Roll & gameMode != GameModeId.GameOver;
            for (int i = 0; i < UtilityButton.Count; i++)
                _utilityButtons[i].Unlock = unlock;
        }

        #region Constants
        // ------------------------------------------
        private readonly struct Window
        {
            public const int Perks    = 0;
            public const int Exchange = 1;
            public const int Settings = 2;
            public const int Help     = 3;

            public const int Count = 4;
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

            EUtility.SetArray(ref _windows, Window.Count);
            _windows[Window.Perks]?.OnValidate("Perks");
            _windows[Window.Exchange]?.OnValidate("Exchange");
            _windows[Window.Settings]?.OnValidate("Settings");
            _windows[Window.Help]?.OnValidate("Help");

            EUtility.SetObject(ref _giftWindow);
            EUtility.SetObjects(ref _giftButtons);

            EUtility.SetObject(ref _diceWindow);

            EUtility.SetObject(ref _gameOverWindow);
        }
#endif
    }
}
