//Assets\Colonization\Scripts\UI\_UIGame\ContextMenus\CrossroadMainMenu.cs
using UnityEngine;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class CrossroadMainMenu : AWorldMenu
    {
        [Space]
        [SerializeField] private float _distanceOfButtons = 5f;
        [Space]
        [SerializeField] private WorldHintButton _buttonClose;
        [Space]
        [SerializeField] private ButtonBuildEdifice _buttonUpgrade;
        [SerializeField] private WorldHintButton _buttonRecruiting;
        [SerializeField] private ButtonBuild _buttonWall;
        [SerializeField] private ButtonBuild _buttonRoads;

        private Crossroad _currentCrossroad;
        private CrossroadRoadsMenu _roadsMenu;
        private CrossroadWarriorsMenu _warriorsMenu;
        private Human _player;

        public ISigner<IMenu, bool> Init(CrossroadRoadsMenu roadsMenu, CrossroadWarriorsMenu warriorsMenu, ContextMenuSettings settings)
        {
            _roadsMenu = roadsMenu;
            _warriorsMenu = warriorsMenu;
            _player = settings.player;

            _buttonClose.Init(settings.hint, Close);

            _buttonRoads.Init(new(0f, -_distanceOfButtons, 0f), settings, settings.prices.Road, OnRoads);

            Vector3 distance60angle = new(_distanceOfButtons * CONST.SIN_60, _distanceOfButtons * CONST.COS_60, 0f);
            _buttonRecruiting.Init(distance60angle, settings.hint, OnHiring);
            _buttonWall.Init(distance60angle, settings, settings.prices.Wall, OnWall);
            distance60angle.x *= -1f;
            _buttonUpgrade.Init(distance60angle, settings, settings.prices.Edifices, OnUpgrade);

            base.CloseInstant();

            return _eventActive;
        }

        public void Open(Crossroad crossroad)
        {
            _currentCrossroad = crossroad;

            _buttonRecruiting.Setup(_player.CanAnyRecruiting(crossroad));
            _buttonUpgrade.Setup(_player.CanEdificeUpgrade(crossroad), crossroad.NextId.Value);
            _buttonWall.Setup(_player.CanWallBuild(crossroad));
            _buttonRoads.Setup(_player.CanRoadBuild(crossroad));

            base.Open();
        }

        protected override void Enable()
        {
            base.Enable();
            _currentCrossroad.SetCaptionHexagonsActive(true);
        }
        protected override void Disable()
        {
            _currentCrossroad.SetCaptionHexagonsActive(false);
            base.Disable();
        }

        private void OnUpgrade()
        {
            base.Close();
            _player.BuyEdificeUpgrade(_currentCrossroad);
        }

        private void OnWall()
        {
            base.Close();
            _player.BuyWall(_currentCrossroad);
        }

        private void OnRoads()
        {
            base.Close();
            _roadsMenu.Open(_currentCrossroad);
        }

        private void OnHiring()
        {
            base.Close();
            _warriorsMenu.Open(_currentCrossroad);
        }
    }
}
