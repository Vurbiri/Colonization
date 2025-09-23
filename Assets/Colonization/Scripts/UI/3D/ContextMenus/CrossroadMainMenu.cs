using UnityEngine;
using Vurbiri.Reactive;

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

        public Event<IMenu, bool> Init(CrossroadRoadsMenu roadsMenu, CrossroadWarriorsMenu warriorsMenu)
        {
            _roadsMenu = roadsMenu;
            _warriorsMenu = warriorsMenu;

            _buttonClose.Init(Close);

            var prices = GameContainer.Prices;
            _buttonUpgrade.Init(prices.Edifices, OnUpgrade);
            _buttonRecruiting.Init(OnHiring);
            _buttonWall.Init(prices.Wall, OnWall);
            _buttonRoads.Init(prices.Road, OnRoads);

            base.CloseInstant();

            return _eventActive;
        }

        public void Open(Crossroad crossroad)
        {
            _currentCrossroad = crossroad;

            var person = GameContainer.Players.Person;
            _buttonRecruiting.Setup(person.CanAnyRecruiting(crossroad));
            _buttonUpgrade.Setup(person.CanEdificeUpgrade(crossroad), person.IsEdificeUnlock(crossroad.NextId), crossroad.NextId.Value);
            _buttonWall.Setup(person.CanWallBuild(crossroad), person.IsWallUnlock());
            _buttonRoads.Setup(person.CanRoadBuild(crossroad));

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
            GameContainer.Players.Person.BuyEdificeUpgrade(_currentCrossroad);
        }

        private void OnWall()
        {
            base.Close();
            GameContainer.Players.Person.BuyWall(_currentCrossroad);
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
            this.SetChildren(ref _buttonClose, "ButtonClose");
            this.SetChildren(ref _buttonUpgrade);
            this.SetChildren(ref _buttonRecruiting, "ButtonRecruiting");
            this.SetChildren(ref _buttonWall, "ButtonBuildWall");
            this.SetChildren(ref _buttonRoads, "ButtonBuildRoads");
        }
#endif
    }
}
