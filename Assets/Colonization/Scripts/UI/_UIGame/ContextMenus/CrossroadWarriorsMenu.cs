using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Actors;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class CrossroadWarriorsMenu : AWorldMenu
    {
        [Space]
        [SerializeField] private WorldHintButton _buttonBack;
        [Space]
        [SerializeField] private IdSet<WarriorId, ButtonRecruiting> _buttons;

        private CrossroadMainMenu _mainMenu;

        public ISubscription<IMenu, bool> Init(ContextMenuSettings settings, CrossroadMainMenu mainMenu)
        {
            var warriorPrices = settings.prices.Warriors;
            _mainMenu = mainMenu;

            _buttonBack.Init(settings.hint, OnClose);

            for (int i = 0; i < WarriorId.Count; i++)
                _buttons[i].Init(settings, warriorPrices[i], this);

            base.CloseInstant();

            return _eventActive;
        }

        public void Open(Crossroad crossroad)
        {
            foreach (var button in _buttons)
                button.Setup(crossroad);

            base.Open();
        }

        private void OnClose()
        {
            base.Close();
            _mainMenu.Open();
        }

#if UNITY_EDITOR

        public override void SetButtonPosition(float buttonDistance)
        {
            _buttonBack.transform.localPosition = Vector3.zero;

            float angle = 360 / WarriorId.Count;
            Vector3 distance = new(0f, buttonDistance, 0f);
            for (int i = 0; i < WarriorId.Count; i++)
                _buttons[i].transform.localPosition = Quaternion.Euler(0f, 0f, -angle * i) * distance;
        }

        private void OnValidate()
        {
            if (_buttonBack == null)
                _buttonBack = GetComponentInChildren<WorldHintButton>();
            if (_buttons.Fullness < _buttons.Count)
                _buttons.ReplaceRange(GetComponentsInChildren<ButtonRecruiting>());
        }
#endif
    }

}
