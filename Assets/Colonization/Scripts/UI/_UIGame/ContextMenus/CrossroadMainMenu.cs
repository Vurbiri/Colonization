//Assets\Colonization\Scripts\UI\_UIGame\ContextMenus\CrossroadMainMenu.cs
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class CrossroadMainMenu : ACrossroadMenuBuild
    {
        [Space]
        [SerializeField] private HintingButton _buttonClose;
        [Space]
        [SerializeField] private ButtonBuildEdifice _buttonUpgrade;
        [SerializeField] private HintingButton _buttonRecruiting;
        [SerializeField] private ButtonBuild _buttonWall;
        [SerializeField] private ButtonBuild _buttonRoads;

        private CrossroadRoadsMenu _roadsMenu;
        private CrossroadWarriorsMenu _warriorsMenu;
        private Players _players;
        private Player _playerCurrent;

        public void Init(CrossroadRoadsMenu roadsMenu, CrossroadWarriorsMenu warriorsMenu, ContextMenuSettings settings)
        {
            _roadsMenu = roadsMenu;
            _warriorsMenu = warriorsMenu;
            _players = settings.players;

            _buttonClose.Init(settings.hint, OnClose);

            _buttonRoads.Init(new(0f, -_distanceOfButtons, 0f), settings, settings.prices.Road, OnRoads);

            Vector3 distance60angle = new(_distanceOfButtons * CONST.SIN_60, _distanceOfButtons * CONST.COS_60, 0f);
            _buttonRecruiting.Init(distance60angle, settings.hint, settings.color, OnHiring);
            _buttonWall.Init(distance60angle, settings, settings.prices.Wall, OnWall);
            distance60angle.x *= -1f;
            _buttonUpgrade.Init(distance60angle, settings, settings.prices.Edifices, OnUpgrade);

            _thisGO.SetActive(false);
        }

        public override void Open(Crossroad crossroad)
        {
            _playerCurrent = _players.Current;
            _currentCrossroad = crossroad;

            ACurrencies currentCash = _playerCurrent.Resources;

            _buttonRecruiting.Setup(_playerCurrent.CanAnyRecruitingWarriors(crossroad));
            _buttonUpgrade.Setup(_playerCurrent.CanEdificeUpgrade(crossroad), crossroad.NextId.Value, currentCash);
            _buttonWall.Setup(_playerCurrent.CanWallBuild(crossroad), currentCash);
            _buttonRoads.Setup(_playerCurrent.CanRoadBuild(crossroad), currentCash);

            _thisGO.SetActive(true);
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
