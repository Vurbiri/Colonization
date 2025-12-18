using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.UI;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
	public class SettingsWindow : ASwitchableWindow
    {
        [Space]
        [SerializeField] private IdArray<MixerId, VSliderFloat> _sounds;
        [SerializeField] private VSliderInt _quality;
        [SerializeField] private LanguageSwitch _language;
        [Space]
        [SerializeField] private SimpleButton _closeButton;

        private bool _isApply;
        protected bool _isSaveSettings = true;

        public override Switcher Init()
        {
            _switcher.Init(this, true);

            for (int i = 0; i < MixerId.Count; ++i)
            {
                int index = i;
                _sounds[i].AddListener((value) => ProjectContainer.Settings.Volumes[index] = value, false);
            }
            _quality.AddListener(QualitySettings.SetQualityLevel, false);

            _switcher.onClose.Add(OnClose);
            _switcher.onOpen.Add(OnOpen);

            _closeButton.AddListener(Cancel);
            _closeButton = null;

            return _switcher;
        }

        public void Cancel()
        {
            _isApply = false;
            _switcher.Close();
        }

        public void Apply()
        {
            _isApply = true;
            _switcher.Close();
        }

        private void OnClose()
        {
            if (_isApply)
            {
                ProjectContainer.Settings.Apply();
                if (_isSaveSettings)
                    ProjectContainer.StorageService.Save();
            }
            else
            {
                ProjectContainer.Settings.Cancel();
            }
        }
        private void OnOpen()
        {
            _isApply = false;

            _language.ItemsUpdate();

            _quality.SilentValue = ProjectContainer.Settings.Profile.Quality;

            var volumes = ProjectContainer.Settings.Volumes;
            for (int i = 0; i < MixerId.Count; ++i)
                _sounds[i].SilentValue = volumes[i];
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            base.OnValidate();

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

            this.SetChildren(ref _closeButton);
        }
#endif
    }
}
