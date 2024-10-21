using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class CrossroadMainMenu : ACrossroadMenuBuild
    {
        [Space]
        [SerializeField] private CmButton _buttonClose;
        [Space]
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

            Vector3 distance = Vector3.up * _distanceOfButtons;
            Vector3 distance60angle = Quaternion.Euler(0f, 0f, -60f) * distance;

            _buttonHiring.onClick.AddListener(OnHiring);
            _buttonHiring.transform.localPosition = distance60angle;

            _buttonRoads.Init(Quaternion.Euler(0f, 0f, 180f) * distance, prices.Road);
            _buttonRoads.AddListener(OnRoads);

            _buttonWall.Init(distance60angle, prices.Wall);
            _buttonWall.AddListener(OnWall);

            _buttonUpgrade.Init(Quaternion.Euler(0f, 0f, 60f) * distance, prices.Edifices);
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
            _playerCurrent = _players.Current;
            _currentCrossroad = crossroad;

            ACurrencies currentCash = _playerCurrent.Resources;
            Color currentColor = _playerCurrent.Color;

            _buttonHiring.SetActive(_playerCurrent.CanHiringWarriors(crossroad));
            _buttonHiring.targetGraphic.color = currentColor;

            _buttonUpgrade.Setup(_playerCurrent.CanEdificeUpgrade(crossroad), crossroad.NextId, currentColor, currentCash);
            _buttonWall.Setup(_playerCurrent.CanWallBuild(crossroad), currentColor, currentCash);
            _buttonRoads.Setup(_playerCurrent.CanRoadBuild(crossroad), currentColor, currentCash);

            _thisGO.SetActive(true);
        }
    }
}
