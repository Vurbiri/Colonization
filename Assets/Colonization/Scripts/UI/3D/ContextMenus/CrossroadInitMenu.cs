using UnityEngine;

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
            _buttonInit.Init(OnBuildPort);

            base.CloseInstant();

            return _eventActive;
        }

        public void Open(Crossroad crossroad)
        {
            _currentCrossroad = crossroad;
            bool buttonEnable = GameContainer.Person.CanEdificeUpgrade(crossroad) & crossroad.NextGroupId == EdificeGroupId.Port;
            _buttonInit.Setup(buttonEnable, crossroad.NextId.Value);
            base.Open();
        }

        protected override void Enable()
        {
            base.Enable();
            _currentCrossroad.CaptionHexagonsEnable();
        }
        protected override void Disable()
        {
            _currentCrossroad.CaptionHexagonsDisable();
            base.Disable();

            if (_endInit)
            {
                GameContainer.GameLoop.EndLanding();
                Destroy(gameObject);
            }
        }

        private void OnBuildPort()
        {
            base.Close();
            _endInit = GameContainer.Person.BuildPort(_currentCrossroad);
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
