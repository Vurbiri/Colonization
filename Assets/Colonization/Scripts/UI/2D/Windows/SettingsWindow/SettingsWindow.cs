using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.UI;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
	public class SettingsWindow : MonoBehaviour
	{
        [SerializeField] protected Switcher _switcher;
        [Space]
        [SerializeField] private IdArray<MixerId, VSliderFloat> _sounds;
        [SerializeField] private VSliderInt _quality;
        [SerializeField] private LanguageSwitch _language;
        [Space]
        [SerializeField] protected SimpleButton _closeButton;

        private bool _isApply;

        public virtual Switcher Init()
        {
            _switcher.Init(this);

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
                ProjectContainer.Settings.Apply();
            else 
                ProjectContainer.Settings.Cancel();

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
        protected virtual void OnValidate()
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

            this.SetChildren(ref _closeButton);
        }

        public void UpdateVisuals_Ed(float pixelsPerUnit, ProjectColors colors)
        {
            var color = colors.PanelBack.SetAlpha(1f);
            var mainImage = GetComponent<UnityEngine.UI.Image>();

            mainImage.color = color;
            mainImage.pixelsPerUnitMultiplier = pixelsPerUnit;
        }
#endif
    }
}
