using TMPro;
using UnityEngine;

namespace Vurbiri.International.UI
{
	[AddComponentMenu("UI/TextMeshPro - Text (UI Localization)", 12)]
	public class TextMeshProUGUIL : TextMeshProUGUI
	{
		[SerializeField] private FileIdAndKey _getText;
		[SerializeField] private bool _extract;

		private Subscription _subscribe;

		public void SetKey(FileId file, string key, bool extract = false)
		{
			_getText = new(file, key);
			_extract = extract;

			if (_subscribe == null)
				_subscribe = Localization.Subscribe(SetText);
			else
				text = Localization.Instance.GetText(_getText, _extract);
		}

		protected override void Start()
		{
			base.Start();
#if UNITY_EDITOR
			if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
#endif
			_subscribe ??= Localization.Subscribe(SetText);
		}

		private void SetText(Localization localization)
		{
			text = localization.GetText(_getText, _extract);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			_subscribe?.Dispose();
		}

#if UNITY_EDITOR
		public const string getTextField = nameof(_getText);
		public const string extractField = nameof(_extract);
#endif
	}
}
