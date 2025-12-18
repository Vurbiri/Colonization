using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
    sealed public class ButtonCancel : AHintButton3D, IMenu
    {
        private ICancel _cancelledObj;
        private Subscription _unAction;

        private readonly VAction<IMenu, bool> _changeEvent = new();

        public Event<IMenu, bool> Init()
        {
            base.InternalInit(OnClick, false);
            Localization.Subscribe(SetText);
            
            return _changeEvent;
        }

        public void Setup(ICancel cancelledObj)
        {
            _unAction?.Dispose();

            _cancelledObj = cancelledObj;
            _unAction = _cancelledObj.CanCancel.Subscribe(_thisGameObject.SetActive);
        }

        public void CloseInstant()
        {
            _thisGameObject.SetActive(false);
            _unAction?.Dispose();
            _cancelledObj = null;
        }

        private void OnClick()
        {
            _thisGameObject.SetActive(false);
            _unAction?.Dispose();
            _cancelledObj?.Cancel();
            _cancelledObj = null;
        }

        private void SetText(Localization localization) => _hintText = localization.GetText(LangFiles.Main, "Cancel");

        protected override void OnEnable()
        {
            base.OnEnable();
            _changeEvent.Invoke(this, true);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            _changeEvent.Invoke(this, false);
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            Localization.Unsubscribe(SetText);
            _unAction?.Dispose();
        }
    }
}
