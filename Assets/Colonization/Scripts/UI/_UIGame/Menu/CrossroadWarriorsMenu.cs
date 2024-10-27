using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class CrossroadWarriorsMenu : ACrossroadMenuBuild
    {
        [Space]
        [SerializeField] private CmButton _buttonBack;
        [Space]
        [SerializeField] private IdHashSet<WarriorId, ButtonRecruiting> _buttons;

        private CrossroadMainMenu _mainMen;

        public void Init(CrossroadMainMenu mainMenu, Players players, IReadOnlyList<ACurrencies> warriorPrices)
        {
            _mainMen = mainMenu;

            _buttonBack.onClick.AddListener(OnBack);

            float angle = 360 / WarriorId.Count;
            Vector3 distance = new(0f, _distanceOfButtons, 0f);
            for (int i = 0; i < WarriorId.Count; i++)
                _buttons[i].Init(Quaternion.Euler(0f, 0f, -angle * i) * distance, players, warriorPrices[i]).AddListener(OnClick);

            _thisGO.SetActive(false);
        }

        public override void Open(Crossroad crossroad)
        {
            foreach (var button in _buttons)
                button.Setup(crossroad);

             _thisGO.SetActive(true);
        }

        private void OnBack()
        {
            _thisGO.SetActive(false);
            _mainMen.Open();
        }

        private void OnClick()
        {
            _thisGO.SetActive(false);
        }
    }

}
