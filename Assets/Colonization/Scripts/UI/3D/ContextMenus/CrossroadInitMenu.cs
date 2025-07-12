using System.Collections;
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
        private GameLoop _game;
        private Human _player;
        private bool _endInit = false;

        public ISubscription<IMenu, bool> Init(ContextMenuSettings settings)
        {
            _game = settings.game;
            _player = settings.player;

            _buttonClose.Init(settings.hint, Close);
            _buttonInit.Init(settings, OnUpgrade);

            base.CloseInstant();

            return _eventActive;
        }

        public void Open(Crossroad crossroad)
        {
            _currentCrossroad = crossroad;
            bool buttonEnable = _player.CanEdificeUpgrade(crossroad) & crossroad.NextGroupId == EdificeGroupId.Port;
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
                StartCoroutine(OnEndLanding_Cn());
        }

        private void OnUpgrade()
        {
            base.Close();
            _endInit = _player.BuildPort(_currentCrossroad);
        }

        private IEnumerator OnEndLanding_Cn()
        {
            yield return _game.EndLanding();
            Destroy(gameObject);
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
