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
        private Players _players;
        private Player _playerCurrent;

        public void Init(CrossroadRoadsMenu roadsMenu, CrossroadWarriorsMenu warriorsMenu, Players players, PricesScriptable prices)
        {
            _roadsMenu = roadsMenu;
            _warriorsMenu = warriorsMenu;
            _players = players;

            _buttonClose.onClick.AddListener(OnClose);

            _buttonRoads.Init(new(0f, -_distanceOfButtons, 0f), prices.Road).AddListener(OnRoads);

            Vector3 distance60angle = new(_distanceOfButtons * CONST.SIN_60, _distanceOfButtons * CONST.COS_60, 0f);

            _buttonHiring.transform.localPosition = distance60angle;
            _buttonHiring.onClick.AddListener(OnHiring);

            _buttonWall.Init(distance60angle, prices.Wall).AddListener(OnWall);

            distance60angle.x *= -1f;
            _buttonUpgrade.Init(distance60angle, prices.Edifices).AddListener(OnUpgrade);

            _thisGO.SetActive(false);
        }

        public override void Open(Crossroad crossroad)
        {
            _playerCurrent = _players.Current;
            _currentCrossroad = crossroad;

            ACurrencies currentCash = _playerCurrent.Resources;
            Color currentColor = _playerCurrent.Visual.color;

            _buttonHiring.SetActive(_playerCurrent.CanAnyRecruitingWarriors(crossroad));
            _buttonHiring.targetGraphic.color = currentColor;

            _buttonUpgrade.Setup(_playerCurrent.CanEdificeUpgrade(crossroad), crossroad.NextId, currentColor, currentCash);
            _buttonWall.Setup(_playerCurrent.CanWallBuild(crossroad), currentColor, currentCash);
            _buttonRoads.Setup(_playerCurrent.CanRoadBuild(crossroad), currentColor, currentCash);

            _thisGO.SetActive(true);
        }

        private void OnClose()
        {
            _thisGO.SetActive(false);
        }

        private void OnUpgrade()
        {
            _thisGO.SetActive(false);
            _playerCurrent.BuyEdificeUpgrade(_currentCrossroad);
        }

        private void OnWall()
        {
            _thisGO.SetActive(false);
            _playerCurrent.BuyWall(_currentCrossroad);
        }

        private void OnRoads()
        {
            _thisGO.SetActive(false);
            _roadsMenu.Open(_currentCrossroad);
        }

        private void OnHiring()
        {
            _thisGO.SetActive(false);
            _warriorsMenu.Open(_currentCrossroad);
        }
    }
}
