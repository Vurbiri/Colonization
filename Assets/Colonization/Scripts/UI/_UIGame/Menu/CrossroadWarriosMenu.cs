using System.Collections.Generic;
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class CrossroadWarriorsMenu : ACrossroadMenuBuild
    {
        [Space]
        [SerializeField] private CmButton _buttonBack;
        [Space]
        [SerializeField] private IdHashSet<WarriorId, ButtonHiring> _buttons;

        private CrossroadMainMenu _mainMen;

        public void Init(CrossroadMainMenu mainMenu, Players players, IReadOnlyList<ACurrencies> warriorPrices)
        {
            _mainMen = mainMenu;
            _players = players;

            _buttonBack.onClick.AddListener(OnBack);

            Transform thisTransform = transform;
            float angle = 360 / WarriorId.Count;
            Vector3 distance = Vector3.up * _distanceOfButtons;

            for (int i = 0; i < WarriorId.Count; i++)
            {
                _buttons[i].Init(Quaternion.Euler(0f, 0f, -angle * i) * distance, warriorPrices[i]);
            }


            _thisGO.SetActive(false);

            #region Local: OnBack()
            //=================================
            void OnBack()
            {
                _thisGO.SetActive(false);
                _mainMen.Open();
            }
            #endregion
        }

        public override void Open(Crossroad crossroad)
        {
            _playerCurrent = _players.Current;
            _currentCrossroad = crossroad;

            ACurrencies currentCash = _playerCurrent.Resources;
            Color currentColor = _playerCurrent.Color;

            foreach (var button in _buttons)
                button.Setup(currentColor, currentCash);

             _thisGO.SetActive(true);
        }
      
    }

}
