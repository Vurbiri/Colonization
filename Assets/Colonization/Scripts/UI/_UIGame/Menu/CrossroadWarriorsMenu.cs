namespace Vurbiri.Colonization.UI
{
    using Actors;
    using System.Collections.Generic;
    using UnityEngine;
    using Vurbiri.Collections;
    using Vurbiri.UI;

    public class CrossroadWarriorsMenu : ACrossroadMenuBuild
    {
        [Space]
        [SerializeField] private HintingButton _buttonBack;
        [Space]
        [SerializeField] private IdHashSet<WarriorId, ButtonRecruiting> _buttons;

        private CrossroadMainMenu _mainMen;

        public void Init(CrossroadMainMenu mainMenu, Players players, Color color, IReadOnlyList<ACurrencies> warriorPrices)
        {
            _mainMen = mainMenu;

            _buttonBack.Init(OnClose);

            float angle = 360 / WarriorId.Count;
            Vector3 distance = new(0f, _distanceOfButtons, 0f);
            for (int i = 0; i < WarriorId.Count; i++)
                _buttons[i].Init(players, color, warriorPrices[i], _thisGO, Quaternion.Euler(0f, 0f, -angle * i) * distance);

            _thisGO.SetActive(false);
        }

        public override void Open(Crossroad crossroad)
        {
            foreach (var button in _buttons)
                button.Setup(crossroad);

             _thisGO.SetActive(true);
        }

        protected override void OnClose()
        {
            _thisGO.SetActive(false);
            _mainMen.Open();
        }
    }

}
