using Vurbiri.International;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class ButtonCancel : AHintButton3D, IMenu
    {
        private ICancel _cancelledObj;
        private Unsubscription _unLanguage, _unAction;

        private readonly Subscription<IMenu, bool> _subscriber = new();

        public ISubscription<IMenu, bool> Init(WorldHint hint)
        {
            base.Init(hint, OnClick, false);
            _unLanguage = Localization.Instance.Subscribe(SetText);
            
            return _subscriber;
        }

        public void Setup(ICancel cancelledObj)
        {
            _unAction?.Unsubscribe();

            _cancelledObj = cancelledObj;
            _unAction = _cancelledObj.CanCancel.Subscribe(_thisGameObject.SetActive);
        }

        public void CloseInstant()
        {
            _thisGameObject.SetActive(false);
            _unAction?.Unsubscribe();
            _cancelledObj = null;
        }

        private void OnClick()
        {
            _thisGameObject.SetActive(false);
            _unAction?.Unsubscribe();
            _cancelledObj?.Cancel();
            _cancelledObj = null;
        }

        private void SetText(Localization localization) => _text = localization.GetText(LangFiles.Main, "Cancel");

        protected override void OnEnable()
        {
            base.OnEnable();
            _subscriber.Invoke(this, true);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            _subscriber.Invoke(this, false);
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            _unLanguage?.Unsubscribe();
            _unAction?.Unsubscribe();
        }
    }
}
