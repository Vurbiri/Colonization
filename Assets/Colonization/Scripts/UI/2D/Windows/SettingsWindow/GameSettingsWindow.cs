using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.UI;
using Vurbiri.EntryPoint;
using Vurbiri.International;
using Vurbiri.Reactive;
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
		[SerializeField, Key(Gameplay)] private string _isExit;

		private readonly ReactiveFlags _flags = new();
		private readonly WaitSignal _waitSaving = new();
		private Coroutine _coroutine;

		public override Switcher Init()
		{
			var players = GameContainer.Players;
			for (int i = 0; i < PlayerId.Count; ++i)
				_flags.Add(players[i].Interactable);

			_flags.Subscribe((value) => _isSaveSettings = value);
			_flags.Subscribe(_saveButton.GetSetor<bool>(nameof(HintButton.interactable)));
			_saveButton.AddListener(Save);
			_saveButton = null;

			return base.Init();
		}
		
		public void ExitToMenu()
		{
			_coroutine ??= StartCoroutine(Exit_Cn());
		}

		private void Save()
		{
			_coroutine ??= StartCoroutine(Save_Cn());
		}

		private IEnumerator Exit_Cn()
		{
			_switcher.Close();

			bool saved = ProjectContainer.StorageService.IsSaved;

			if (!saved & _flags.Value)
			{
				yield return Save_Cn(Out<bool>.Get(out int key));
				saved = Out<bool>.Result(key);
			}

			if(!saved)
			{
				yield return MessageBox.Open(Localization.Instance.GetText(Gameplay, _isExit), out WaitButton wait, MBButton.OkCancel);
				saved = wait.Id == MBButtonId.Ok;
			}

			if (saved)
				Transition.Exit();

			_coroutine = null;
		}

		private IEnumerator Save_Cn()
		{
			_switcher.Close();

			yield return Save_Cn(Out<bool>.Get(out int key));
			yield return null;

			if (Out<bool>.Result(key))
				Banner.Open(Localization.Instance.GetText(Gameplay, _goodSave), MessageTypeId.Profit, 3f);
			else
				Banner.Open(Localization.Instance.GetText(Gameplay, _errorSave), MessageTypeId.Error, 3f);

			_coroutine = null;
		}

		private IEnumerator Save_Cn(Out<bool> output)
		{
			Banner.Open(Localization.Instance.GetText(Gameplay, _saving), MessageTypeId.Info, _waitSaving.Restart());

			yield return ProjectContainer.StorageService.Save(out WaitResult<bool> save);

			_waitSaving.Send();
			output.Set(save);
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
