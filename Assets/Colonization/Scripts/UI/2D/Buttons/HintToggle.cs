using System;
using UnityEngine;
using Vurbiri.International;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
#if UNITY_EDITOR
    [AddComponentMenu(VUI_CONST_ED.MENU_NAME + "Hint Toggle", VUI_CONST_ED.TOGGLE_ORDER)]
#endif
    sealed public class HintToggle : AHintToggle<HintToggle>
    {
        private const float RATIO = 0.5f;
        
        [SerializeField] private FileIdAndKey _getText;
        [SerializeField] private bool _extract;

        public void Init(bool value, Action<bool> action)
        {
            base.InternalInit(HintId.Canvas, value, action, RATIO);
            Localization.Subscribe(SetLocalizationText);
        }
        public void Init(RBool rBool)
        {
            base.InternalInit(HintId.Canvas, rBool, rBool.GetSetor<bool>(nameof(rBool.Value)), RATIO);
            Localization.Subscribe(SetLocalizationText);
        }

        private void SetLocalizationText(Localization localization)
        {
            _hintText = localization.GetText(_getText, _extract);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Localization.Unsubscribe(SetLocalizationText);
        }
    }
}
