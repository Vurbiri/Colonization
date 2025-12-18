using UnityEngine;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
#if UNITY_EDITOR
    [AddComponentMenu(VUI_CONST_ED.MENU_NAME + "Hint Button", VUI_CONST_ED.BUTTON_ORDER)]
#endif
    sealed public class HintButton : AHintButton
    {
        [SerializeField] private FileIdAndKey _getText;
        [SerializeField] private bool _extract;

        protected override void Start()
        {
            base.Start();
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            base.InternalInit(HintId.Canvas, 0.505f);
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
