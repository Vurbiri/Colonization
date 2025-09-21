using System;
using UnityEngine;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class HintButton : AHintButton<CanvasHint>
    {
        [SerializeField] private FileIdAndKey _getText;

        public void Init(Action action)
        {
            base.InternalInit(GameContainer.UI.CanvasHint, action, 0.5f);

            Localization.Instance.Subscribe(SetLocalizationText);
        }

        private void SetLocalizationText(Localization localization)
        {
            _hintText = localization.GetText(_getText.id, _getText.key);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Localization.Instance.Unsubscribe(SetLocalizationText);
        }
    }
}
