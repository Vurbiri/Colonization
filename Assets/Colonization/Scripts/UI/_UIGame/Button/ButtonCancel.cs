//Assets\Colonization\Scripts\UI\_UIGame\Button\ButtonCancel.cs
using Vurbiri.Reactive;
using Vurbiri.TextLocalization;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class ButtonCancel : AHintingButton
    {
        private const Files FILE = Files.Main;
        private const string KEY = "Cancel";

        private ICancel _cancelledObj;
        private Unsubscriber _unLanguage, _unAction;

        public void Init(HintGlobal hint)
        {
            base.Init(hint, OnClick, false);
            _unLanguage = SceneServices.Get<Localization>().Subscribe(SetText);
        }

        public void Setup(ICancel cancelledObj)
        {
            _unAction?.Unsubscribe();

            _cancelledObj = cancelledObj;
            _unAction = _cancelledObj.CanCancel.Subscribe(_thisGO.SetActive);
        }

        public void Disable()
        {
            _thisGO.SetActive(false);
            _unAction?.Unsubscribe();
            _cancelledObj = null;
        }

        private void OnClick()
        {
            _thisGO.SetActive(false);
            _unAction?.Unsubscribe();
            _cancelledObj?.Cancel();
            _cancelledObj = null;
        }

        private void SetText(Localization localization) => _text = localization.GetText(FILE, KEY);

        private void OnDestroy()
        {
            _unLanguage?.Unsubscribe();
            _unAction?.Unsubscribe();
        }
    }
}
