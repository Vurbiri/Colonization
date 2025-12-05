using UnityEngine;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	sealed public class GiftButton : AHintButton<CanvasHint, Id<PlayerId>>
	{
        [SerializeField] private FileIdAndKey _getText;

        protected override void Start()
        {
            base.Start();
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            base.InternalInit(GameContainer.UI.CanvasHint, 0.55f);
            Localization.Instance.Subscribe(SetLocalizationText);
        }

        private void SetLocalizationText(Localization localization) => _hintText = localization.GetText(_getText.id, _getText.key);

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Localization.Instance.Unsubscribe(SetLocalizationText);
        }
    }
}
