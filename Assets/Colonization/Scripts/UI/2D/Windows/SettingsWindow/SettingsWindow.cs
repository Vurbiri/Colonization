using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.UI;
using Vurbiri.EntryPoint;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
	public class SettingsWindow : MonoBehaviour
	{
        [SerializeField] private HintButton _settingsButton;
        [SerializeField] private Switcher _switcher;
        [SerializeField] private IdArray<MixerId, VSliderFloat> _sounds;
        [SerializeField] private VSliderInt _quality;
        [SerializeField] private LanguageSwitch _language;

        public Switcher Switcher => _switcher;

        private void Start()
		{
            _switcher.Init(this);
            _settingsButton.AddListener(_switcher.Switch);

            for (int i = 0; i < MixerId.Count; ++i)
                SetSound(i, _sounds[i]);
            _sounds = null;

            var profile = ProjectContainer.Settings.Profile;
            _quality.MaxValue = profile.MaxQuality;
            _quality.Value = profile.Quality;
            _quality.AddListener(QualitySettings.SetQualityLevel);
            _quality = null;

            _switcher.onClose.Add(_language.ItemsUpdate);
            _language = null;
        }

        public void Apply()
        {
            _switcher.Close();
            ProjectContainer.Settings.Apply();
        }

        public void Cancel()
        {
            _switcher.Close();
            ProjectContainer.Settings.Cancel();
        }

        public void Save()
        {
            _switcher.Close();
            ProjectContainer.StorageService.Save();
        }
        
		public void Exit()
		{
            _switcher.Close();
            ProjectContainer.StorageService.Save();
			Transition.Exit();
        }
        
        private static void SetSound(int index, VSliderFloat slider)
        {
            slider.Value = ProjectContainer.Settings.Volumes[index];
            slider.AddListener((value) => ProjectContainer.Settings.Volumes[index] = value);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            EUtility.SetObject(ref _settingsButton, "SettingsButton");

            _switcher ??= new();
            _switcher.OnValidate(this);

            this.SetChildren(ref _language);
            this.SetChildren(ref _quality);

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
