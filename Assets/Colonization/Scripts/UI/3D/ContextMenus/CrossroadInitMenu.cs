using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
	public class CrossroadInitMenu : AWorldMenu
	{
        [Space]
        [SerializeField] private WorldHintButton _buttonClose;
        [Space]
        [SerializeField] private ButtonInit _buttonInit;

        private Crossroad _currentCrossroad;
        private bool _endInit = false;

        public Event<IMenu, bool> Init()
        {
            _buttonClose.Init(Close);
            _buttonInit.Init(OnUpgrade);

            base.CloseInstant();

            return _eventActive;
        }

        public void Open(Crossroad crossroad)
        {
            _currentCrossroad = crossroad;
            bool buttonEnable = GameContainer.Players.Person.CanEdificeUpgrade(crossroad) & crossroad.NextGroupId == EdificeGroupId.Port;
            _buttonInit.Setup(buttonEnable, crossroad.NextId.Value);
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

            if (_endInit)
            {
                GameContainer.GameLoop.EndLanding();
                Destroy(gameObject);
            }
        }

        private void OnUpgrade()
        {
            base.Close();
            _endInit = GameContainer.Players.Person.BuildPort(_currentCrossroad);
        }

#if UNITY_EDITOR

        public override void SetButtonPosition(float buttonDistance)
        {

            _buttonClose.transform.localPosition = Vector3.zero;
            _buttonInit.transform.localPosition = new(0f, buttonDistance, 0f);
        }

        private void OnValidate()
        {
            this.SetChildren(ref _buttonClose);
            this.SetChildren(ref _buttonInit);
        }
#endif

    }
}
