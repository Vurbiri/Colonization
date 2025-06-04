using UnityEngine;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class CrossroadMainMenu : AWorldMenu
    {
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

        public ISubscription<IMenu, bool> Init(ContextMenuSettings settings, CrossroadRoadsMenu roadsMenu, CrossroadWarriorsMenu warriorsMenu)
        {
            _roadsMenu = roadsMenu;
            _warriorsMenu = warriorsMenu;
            _player = settings.player;

            _buttonClose.Init(settings.hint, Close);

            _buttonUpgrade.Init(settings, settings.prices.Edifices, OnUpgrade);
            _buttonRecruiting.Init(settings.hint, OnHiring);
            _buttonWall.Init(settings, settings.prices.Wall, OnWall);
            _buttonRoads.Init(settings, settings.prices.Road, OnRoads);

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

#if UNITY_EDITOR

        public override void SetButtonPosition(float buttonDistance)
        {
            _buttonClose.transform.localPosition = Vector3.zero;
            
            _buttonRoads.transform.localPosition = new(0f, -buttonDistance, 0f);
            Vector3 distance60angle = new(buttonDistance * CONST.SIN_60, buttonDistance * CONST.COS_60, 0f);
            _buttonRecruiting.transform.localPosition = distance60angle;
            _buttonWall.transform.localPosition = distance60angle;
            distance60angle.x *= -1f;
            _buttonUpgrade.transform.localPosition = distance60angle;
        }

        private void OnValidate()
        {
            if (_buttonClose == null)
                _buttonClose = EUtility.GetComponentInChildren<WorldHintButton>(this, "ButtonClose");
            if (_buttonUpgrade == null)
                _buttonUpgrade = GetComponentInChildren<ButtonBuildEdifice>();
            if (_buttonRecruiting == null)
                _buttonRecruiting = EUtility.GetComponentInChildren<WorldHintButton>(this, "ButtonRecruiting");
            if (_buttonWall == null)
                _buttonWall = EUtility.GetComponentInChildren<ButtonBuild>(this, "ButtonBuildWall");
            if (_buttonRoads == null)
                _buttonRoads = EUtility.GetComponentInChildren<ButtonBuild>(this, "ButtonBuildRoads");
        }
#endif
    }
}
