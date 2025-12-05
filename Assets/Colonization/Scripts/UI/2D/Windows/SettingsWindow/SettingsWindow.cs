using System;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.UI;
using Vurbiri.EntryPoint;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
	public class SettingsWindow : MonoBehaviour
	{
        [SerializeField] private Switcher _switcher;
        [SerializeField] private IdArray<MixerId, VSliderFloat> _sounds;
        [SerializeField] private VSliderInt _quality;
        [SerializeField] private LanguageSwitch _language;
        [Space]
        [SerializeField] private FileIdAndKey _goodSave;
        [SerializeField] private FileIdAndKey _errorSave;

        private Action[] _actions;
        private int _state;

        public Switcher Init()
		{
            _switcher.Init(this);

            for (int i = 0; i < MixerId.Count; ++i)
            {
                int index = i;
                _sounds[i].AddListener((value) => ProjectContainer.Settings.Volumes[index] = value, false);
            }
            _quality.AddListener(QualitySettings.SetQualityLevel, false);

            _actions = new Action[] { ProjectContainer.Settings.Cancel, ProjectContainer.Settings.Apply, SaveInternal, ExitInternal };

            _switcher.onClose.Add(OnClose);
            _switcher.onOpen.Add(OnOpen);

            return _switcher;
        }

        public void Cancel()
        {
            _state = State.Cancel;
            _switcher.Close();
        }

        public void Apply()
        {
            _state = State.Apply;
            _switcher.Close();
        }
        
        public void Save()
        {
            _state = State.Save;
            _switcher.Close();
        }
        
		public void Exit()
		{
            _state = State.Exit;
            _switcher.Close();
        }

        private void OnClose()
        {
            _actions[_state].Invoke();
        }
        private void OnOpen()
        {
            _state = State.Cancel;

            _language.ItemsUpdate();

            _quality.SilentValue = ProjectContainer.Settings.Profile.Quality;

            var volumes = ProjectContainer.Settings.Volumes;
            for (int i = 0; i < MixerId.Count; ++i)
                _sounds[i].SilentValue = volumes[i];
        }

        private void SaveInternal()
        {
            ProjectContainer.StorageService.Save(Out<bool>.Get(out int key));
            if (Out<bool>.Result(key))
                Banner.Open(Localization.Instance.GetText(_goodSave), MessageTypeId.Info, 3f);
            else
                Banner.Open(Localization.Instance.GetText(_errorSave), MessageTypeId.Error, 3f);
        }

        private static void ExitInternal()
        {
            ProjectContainer.StorageService.Save();
            Transition.Exit();
        }

        #region Constants
        // ------------------------------------------
        private readonly struct State
        {
            public const int Cancel = 0;
            public const int Apply  = 1;
            public const int Save   = 2;
            public const int Exit   = 3;
        }
        // ------------------------------------------
        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            

            _switcher ??= new();
            _switcher.OnValidate(this);

            this.SetChildren(ref _language);
            this.SetChildren(ref _quality);

            _quality.MaxValue = Profile.MaxQuality;

            for (int i = 0; i < MixerId.Count; ++i)
            {
                if (_sounds[i] == null)
                    _sounds[i] = this.GetComponentInChildren<VSliderFloat>($"{MixerId.Names_Ed[i]}Sound");
                _sounds[i].MinValue = AudioMixer<MixerId>.MIN_VALUE;
                _sounds[i].MaxValue = AudioMixer<MixerId>.MAX_VALUE;
            }
        }
#endif
	}
}
