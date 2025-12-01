using System;
using UnityEngine;
using Vurbiri.International;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class HintToggle : AHintToggle<HintToggle>
    {
        private const float RATIO = 0.5f;
        
        [SerializeField] private FileIdAndKey _getText;
        [SerializeField] private bool _extract;

        public void Init(bool value, Action<bool> action)
        {
            base.InternalInit(GameContainer.UI.CanvasHint, value, action, RATIO);
            Localization.Instance.Subscribe(SetLocalizationText);
        }
        public void Init(RBool rBool)
        {
            base.InternalInit(GameContainer.UI.CanvasHint, rBool, rBool.GetSetor<bool>(nameof(rBool.Value)), RATIO);
            Localization.Instance.Subscribe(SetLocalizationText);
        }

        private void SetLocalizationText(Localization localization)
        {
            _hintText = localization.GetText(_getText, _extract);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Localization.Instance.Unsubscribe(SetLocalizationText);
        }
    }
}
