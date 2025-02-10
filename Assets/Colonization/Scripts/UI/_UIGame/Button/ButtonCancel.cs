//Assets\Colonization\Scripts\UI\_UIGame\Button\ButtonCancel.cs
using Vurbiri.Localization;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class ButtonCancel : AHintingButton
    {
        private const Files FILE = Files.Main;
        private const string KEY = "Cancel";

        private ICancel _cancelledObj;
        private IUnsubscriber _unLanguage;

        public void Init(HintGlobal hint)
        {
            base.Init(hint, OnClick, false);
            _unLanguage = SceneServices.Get<Language>().Subscribe(SetText);
        }

        public void Setup(ICancel cancelledObj)
        {
            _cancelledObj?.IsCancel.Unsubscribe(_thisGO.SetActive);

            _cancelledObj = cancelledObj;
            _cancelledObj.IsCancel.Subscribe(_thisGO.SetActive);
        }

        public void Disable()
        {
            _thisGO.SetActive(false);
            _cancelledObj?.IsCancel.Unsubscribe(_thisGO.SetActive);
            _cancelledObj = null;
        }

        private void OnClick()
        {
            _thisGO.SetActive(false);
            _cancelledObj?.IsCancel.Unsubscribe(_thisGO.SetActive);
            _cancelledObj?.Cancel();
            _cancelledObj = null;
        }

        private void SetText(Language localization) => _text = localization.GetText(FILE, KEY);

        private void OnDestroy()
        {
            _unLanguage?.Unsubscribe();
            _cancelledObj?.IsCancel.Unsubscribe(_thisGO.SetActive);
        }

    }
}
