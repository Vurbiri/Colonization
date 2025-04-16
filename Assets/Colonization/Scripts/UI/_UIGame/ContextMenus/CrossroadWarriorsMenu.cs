//Assets\Colonization\Scripts\UI\_UIGame\ContextMenus\CrossroadWarriorsMenu.cs
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Actors;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class CrossroadWarriorsMenu : ACrossroadMenuBuild
    {
        [Space]
        [SerializeField] private WorldHintButton _buttonBack;
        [Space]
        [SerializeField] private IdSet<WarriorId, ButtonRecruiting> _buttons;

        private CrossroadMainMenu _mainMen;

        public ISigner<bool> Init(CrossroadMainMenu mainMenu, ContextMenuSettings settings)
        {
            var warriorPrices = settings.prices.Warriors;
            _mainMen = mainMenu;

            _buttonBack.Init(settings.hint, OnClose);

            float angle = 360 / WarriorId.Count;
            Vector3 distance = new(0f, _distanceOfButtons, 0f);
            for (int i = 0; i < WarriorId.Count; i++)
                _buttons[i].Init(settings, warriorPrices[i], this, Quaternion.Euler(0f, 0f, -angle * i) * distance);

            CloseInstant();

            return _eventActive;
        }

        public override void Open(Crossroad crossroad)
        {
            foreach (var button in _buttons)
                button.Setup(crossroad);

            Open();
        }

        protected override void OnClose()
        {
            Close();
            _mainMen.Open();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_buttons.Filling < _buttons.Count)
                _buttons.ReplaceRange(GetComponentsInChildren<ButtonRecruiting>());
        }
#endif
    }

}
