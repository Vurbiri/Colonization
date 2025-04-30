//Assets\Colonization\Scripts\UI\_UIGame\Button\ButtonCancel.cs
using UnityEngine;
using Vurbiri.Reactive;
using Vurbiri.TextLocalization;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class ButtonCancel : AWorldHintButton
    {
        private const Files FILE = Files.Main;
        private const string KEY = "Cancel";

        private ICancel _cancelledObj;
        private Unsubscriber _unLanguage, _unAction;

        private readonly Signer<GameObject, bool> _signer = new();

        public ISigner<GameObject, bool> Init(WorldHint hint)
        {
            base.Init(hint, OnClick, false);
            _unLanguage = SceneContainer.Get<Localization>().Subscribe(SetText);
            
            return _signer;
        }

        public void Setup(ICancel cancelledObj)
        {
            _unAction?.Unsubscribe();

            _cancelledObj = cancelledObj;
            _unAction = _cancelledObj.CanCancel.Subscribe(_thisGameObject.SetActive);
        }

        public void Disable()
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

        private void SetText(Localization localization) => _text = localization.GetText(FILE, KEY);

        protected override void OnEnable()
        {
            base.OnEnable();
            _signer.Invoke(_thisGameObject, true);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            _signer.Invoke(_thisGameObject, false);
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            _unLanguage?.Unsubscribe();
            _unAction?.Unsubscribe();
        }
    }
}
