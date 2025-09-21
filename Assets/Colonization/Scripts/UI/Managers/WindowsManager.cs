using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    [System.Serializable]
    public class WindowsManager
    {
        private const int SWITCHERS_COUNT = 3, BUTTONS_COUNT = 2;
        
        [SerializeField, ReadOnly] private PerksWindow _perksWindow;
        [SerializeField, ReadOnly] private HintButton _perksButton;
        [Space]
        [SerializeField, ReadOnly] private ExchangeWindow _exchangeWindow;
        [SerializeField, ReadOnly] private HintButton _exchangeButton;
        [Space]
        [SerializeField, ReadOnly] private GiftWindow _giftWindow;
        [SerializeField, ReadOnly] private GiftButton[] _giftButtons;
        [Space]
        [SerializeField, ReadOnly] private DiceWindow _diceWindow;

        private readonly Switcher[] _switchers = new Switcher[SWITCHERS_COUNT];
        private AVButtonBase[] _buttons;
        private int _openWindowsCount;

        public void Init()
        {
            _buttons = new AVButtonBase[BUTTONS_COUNT] { _perksButton, _exchangeButton };

            int id = 0;
            _switchers[id] = _perksWindow.Init(_perksButton).Setup(id++, OnOpenWindow, OnCloseWindow);
            _switchers[id] = _exchangeWindow.Init(_exchangeButton).Setup(id++, OnOpenWindow, OnCloseWindow);
            _switchers[id] = _giftWindow.Init(_giftButtons).Setup(id++, OnOpenWindow, OnCloseWindow);

            _diceWindow.Init();

            GameContainer.Players.Person.Interactable.Subscribe(OnInteractable);

            _perksWindow = null; _exchangeWindow = null; _giftWindow = null; _diceWindow = null;
            _perksButton = null; _exchangeButton = null;
        }

        private void OnInteractable(bool interactable)
        {
            for (int i = 0; i < BUTTONS_COUNT; i++)
                _buttons[i].Interactable = interactable;

            for (int i = _giftButtons.Length - 1; i >= 0; i--)
                _giftButtons[i].interactable = interactable;

            if (!interactable)
            {
                for (int i = 0; i < SWITCHERS_COUNT; i++)
                    _switchers[i].Close();
            }
        }

        private void OnOpenWindow(int id)
        {
            if (_openWindowsCount++ == 0)
            {
                GameContainer.InputController.Unselect();
                GameContainer.InputController.WindowMode(true);
            }

            for (int i = 0; i < SWITCHERS_COUNT; i++)
                _switchers[i].TryClose(id);
        }
        private void OnCloseWindow()
        {
            if (--_openWindowsCount == 0)
                GameContainer.InputController.WindowMode(false);
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

            EUtility.SetObject(ref _diceWindow);
        }
#endif
    }
}
