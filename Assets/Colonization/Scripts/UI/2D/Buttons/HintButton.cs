using UnityEngine;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
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
            base.InternalInit(GameContainer.UI.CanvasHint, 0.505f);
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
