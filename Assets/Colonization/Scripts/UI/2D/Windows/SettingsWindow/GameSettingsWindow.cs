using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.UI;
using Vurbiri.EntryPoint;
using Vurbiri.International;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
	public class GameSettingsWindow : SettingsWindow
    {
        [Space]
        [SerializeField] private FileIdAndKey _saving;
        [SerializeField] private FileIdAndKey _goodSave;
        [SerializeField] private FileIdAndKey _errorSave;
        [Space]
        [SerializeField] private HintButton _saveButton;

        private readonly ReactiveFlags _flags = new();
        private readonly WaitSignal _waitSaving = new();
        private Coroutine _coroutine;

        public override Switcher Init()
        {
            var players = GameContainer.Players;
            for (int i = 0; i < PlayerId.Count; ++i)
                _flags.Add(players[i].Interactable);

            _flags.Subscribe(_saveButton.GetSetor<bool>(nameof(HintButton.interactable)));
            _saveButton.AddListener(Save);
            _saveButton = null;

            return base.Init();
        }
        
		public void ExitToMenu()
		{
            Transition.Exit();
        }

        private void Save()
        {
            _coroutine ??= StartCoroutine(Save_Cn());
        }

        private IEnumerator Save_Cn()
        {
            _switcher.Close();
            
            Banner.Open(Localization.Instance.GetText(_saving), MessageTypeId.Info, _waitSaving.Restart());

            var save = ProjectContainer.StorageService.Save();
            yield return save;

            _waitSaving.Send();
            yield return null;

            if (save)
                Banner.Open(Localization.Instance.GetText(_goodSave), MessageTypeId.Profit, 3f);
            else
                Banner.Open(Localization.Instance.GetText(_errorSave), MessageTypeId.Error, 3f);

            _coroutine = null;
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
