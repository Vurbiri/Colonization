using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.Colonization.Storage;
using Vurbiri.EntryPoint;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	public class NewWindow : ASwitchableWindow
	{
		[SerializeField, Key(LangFiles.Main)] private string _lostMsg;
		[Space]
		[SerializeField] private ColorWindow _colorWindow;
		[Space]
		[SerializeField] private PlayerVisualPanel[] _playersVisual;

		private Coroutine _coroutine;
		private WaitButton _button;

		public override Switcher Init()
		{
			_switcher.Init(this, true);
			_switcher.onOpen.Add(OnOpen);
			_switcher.onClose.Add(_colorWindow.Close);

			GetComponentInChildren<SimpleButton>().AddListener(_switcher.Close);

			var playerNames  = ProjectContainer.UI.PlayerNames;
			var playerColors = ProjectContainer.UI.PlayerColors;
			for (int i = 0; i < PlayerId.HumansCount; i++)
				_playersVisual[i].Init(playerNames, playerColors, _colorWindow);

			_colorWindow = null;

            return _switcher;
		}

		public void GameStart()
		{
			_switcher.Close();
			_coroutine ??= StartCoroutine(GameStart_Cn());
		}

		private void OnOpen()
		{
			if(_coroutine != null)
			{
				StopCoroutine(_coroutine);
				_button?.Reset();
				Nullification();
			}
		}

		private IEnumerator GameStart_Cn()
		{
			var game = ProjectContainer.GameSettings;

			if (game.IsLoad)
			{
				_button = MessageBox.Open(Localization.Instance.GetText(LangFiles.Main, _lostMsg), MBButton.OkNo);
				yield return _button;

				if (_button.Id == MBButtonId.No)
				{
					Nullification();
					yield break;
				}

				var names = new string[PlayerId.HumansCount];
				var colors = new Color32[PlayerId.HumansCount];
				for (int i = 0; i < PlayerId.HumansCount; i++)
					(names[i], colors[i]) = _playersVisual[i].Input;
				
				ProjectContainer.UI.PlayerNames.TrySetCustomNames(names);
				ProjectContainer.UI.PlayerColors.TrySetCustomColors(colors);

				int score = 0;
				if (ProjectContainer.StorageService.TryGet(SAVE_KEYS.SCORE, out int[] scores))
					score = scores[PlayerId.Person];

				game.Reset(score);
			}

			Transition.Exit();
		}

		[MethodImpl(256)]
		private void Nullification()
		{
			_coroutine = null;
			_button = null;
		}

		private void OnDestroy()
		{
			for (int i = 0; i < PlayerId.HumansCount; i++)
				_playersVisual[i].Dispose();
		}

		private void OnApplicationQuit() => OnDestroy();

#if UNITY_EDITOR

		public void UpdateVisuals_Ed(float pixelsPerUnit, SceneColorsEd colors)
		{
			GetComponent<UnityEngine.UI.Image>().SetImageFields(colors.panelBack, pixelsPerUnit);

			GetComponentInChildren<SimpleButton>().SetColor_Ed(colors.panelBack);

			var startButton = this.GetComponentInChildren<UnityEngine.UI.Image>("StartButton");
			startButton.SetColorField(colors.menu);
			startButton.GetComponentInChildren<TMPro.TMP_Text>().SetColorField(colors.panelText);
		}

		protected override void OnValidate()
		{
			if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
				return;

			base.OnValidate();

			EUtility.SetObject(ref _colorWindow);

			EUtility.SetArray(ref _playersVisual, PlayerId.HumansCount);
			for (int i = 0; i < PlayerId.HumansCount; i++)
				_playersVisual[i]?.OnValidate(i, this);
		}
#endif
	}
}
