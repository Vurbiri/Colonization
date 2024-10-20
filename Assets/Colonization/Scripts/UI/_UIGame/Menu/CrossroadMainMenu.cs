using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class CrossroadMainMenu : ACrossroadMenuBuild
    {
        [SerializeField] private CmButton _buttonClose;
        [SerializeField] private ButtonBuildEdifice _buttonUpgrade;
        [SerializeField] private CmButton _buttonHiring;
        [SerializeField] private ButtonBuild _buttonWall;
        [SerializeField] private ButtonBuild _buttonRoads;

        private CrossroadRoadsMenu _roadsMenu;
        private CrossroadWarriorsMenu _warriorsMenu;

        public void Init(CrossroadRoadsMenu roadsMenu, CrossroadWarriorsMenu warriorsMenu, Players players, PricesScriptable prices)
        {
            _roadsMenu = roadsMenu;
            _warriorsMenu = warriorsMenu;
            _players = players;

            _buttonClose.onClick.AddListener(() => gameObject.SetActive(false));
            _buttonHiring.onClick.AddListener(OnHiring);

            _buttonRoads.Init(prices.Road);
            _buttonRoads.AddListener(OnRoads);

            _buttonWall.Init(prices.Wall);
            _buttonWall.AddListener(OnWall);

            _buttonUpgrade.Init(prices.Edifices);
            _buttonUpgrade.AddListener(OnUpgrade);

            _thisGO.SetActive(false);

            #region Local: OnUpgrade(), OnWall(), OnRoads()
            //=================================
            void OnUpgrade()
            {
                _thisGO.SetActive(false);
                _playerCurrent.BuyEdificeUpgrade(_currentCrossroad);
            }
            //=================================
            void OnWall()
            {
                _thisGO.SetActive(false);
                _playerCurrent.BuyWall(_currentCrossroad);
            }
            //=================================
            void OnRoads()
            {
                _thisGO.SetActive(false);
                _roadsMenu.Open(_currentCrossroad);
            }
            //=================================
            void OnHiring()
            {
                _thisGO.SetActive(false);
                _warriorsMenu.Open(_currentCrossroad);
            }
            #endregion
        }

        public override void Open(Crossroad crossroad)
        {
            base.Open(crossroad);

            _buttonHiring.SetActive(_playerCurrent.CanHiringWarriors(crossroad));
            _buttonHiring.targetGraphic.color = _currentColor;

            if (ButtonSetup(_buttonUpgrade, _playerCurrent.CanEdificeUpgrade(crossroad)))
                _buttonUpgrade.SetupHint(crossroad.NextId, _playerCurrent.Resources);

            if (ButtonSetup(_buttonWall, _playerCurrent.CanWallBuild(crossroad)))
                _buttonWall.SetupHint(_playerCurrent.Resources);

            if(ButtonSetup(_buttonRoads, _playerCurrent.CanRoadBuild(crossroad)))
                _buttonRoads.SetupHint(_playerCurrent.Resources);

            _thisGO.SetActive(true);
        }
    }
}
