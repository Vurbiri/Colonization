using UnityEngine;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	sealed public class GiftButton : AHintButton<Id<PlayerId>>
	{
		[SerializeField] private FileIdAndKey _getText;

		protected override void Start()
		{
			base.Start();
#if UNITY_EDITOR
			if (!Application.isPlaying) return;
#endif
			base.InternalInit(HintId.Canvas, 0.55f);
			Localization.Subscribe(SetLocalizationText);
		}

		private void SetLocalizationText(Localization localization) => _hintText = localization.GetText(_getText.id, _getText.key);

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Localization.Unsubscribe(SetLocalizationText);
		}
	}
}
