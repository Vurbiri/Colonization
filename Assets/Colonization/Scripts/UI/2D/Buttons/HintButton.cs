using System;
using UnityEngine;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class HintButton : AHintButton
    {
        [SerializeField] private FileIdAndKey _getText;
        [SerializeField] private bool _extract;

        public void Init(Action action)
        {
            base.InternalInit(GameContainer.UI.CanvasHint, action, 0.5f);

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
