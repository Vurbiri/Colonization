using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.UI;
using Vurbiri.International;
using Vurbiri.UI;
using static Vurbiri.Colonization.LangFiles;

namespace Vurbiri.Colonization
{
	public class GameSettingsWindow : SettingsWindow
	{
		[SerializeField] private HintButton _saveButton;
		[Header("Keys")]
		[SerializeField, Key(Gameplay)] private string _saving;
		[SerializeField, Key(Gameplay)] private string _goodSave;
		[SerializeField, Key(Gameplay)] private string _errorSave;

		private readonly WaitSignal _waitSaving = new();
		private Coroutine _coroutine;

		public override Switcher Init()
		{
            _saveButton.AddListener(Save);
            GameContainer.Person.Interactable.Subscribe(OnPlayerInteractable);
			return base.Init();
		}

		private void Save()
		{
            _coroutine ??= StartCoroutine(Save_Cn());
        }

		private IEnumerator Save_Cn()
		{
            _saveButton.interactable = false;

            Banner.Open(Localization.Instance.GetText(Gameplay, _saving), MessageTypeId.Info, _waitSaving.Restart());

            yield return ProjectContainer.StorageService.Save(out WaitResult<bool> save);

            _waitSaving.Send();
            yield return null;

			if (save)
				Banner.Open(Localization.Instance.GetText(Gameplay, _goodSave), MessageTypeId.Profit, 3f);
			else
				Banner.Open(Localization.Instance.GetText(Gameplay, _errorSave), MessageTypeId.Error, 3f);

			_coroutine = null;
		}

		private void OnPlayerInteractable(bool value)
		{
			_isSaveSettings = value;
            _saveButton.interactable = value;
        }

#if UNITY_EDITOR
        protected override void OnValidate()
		{
			if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
				return;

			base.OnValidate();
			this.SetChildren(ref _saveButton, "SaveButton");
		}
#endif
	}
}
