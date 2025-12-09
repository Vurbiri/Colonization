using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.UI;
using Vurbiri.EntryPoint;
using Vurbiri.International;
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

        private readonly WaitSignal _waitSaving = new();
        private Coroutine _coroutine;

        public void Save()
        {
            _coroutine ??= StartCoroutine(Save_Cn());
        }
        
		public void ExitToMenu()
		{
            Transition.Exit();
        }

        private IEnumerator Save_Cn()
        {
            Banner.Open(Localization.Instance.GetText(_saving), MessageTypeId.Info, _waitSaving.Restart());

            var game = GameContainer.GameEvents;
            var players = GameContainer.Players;

            while (!players[game.CurrentPlayer].Interactable)
                yield return null;

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
            base.OnValidate();

            this.SetChildren(ref _saveButton, "SaveButton");
        }
#endif
    }
}
