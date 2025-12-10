using UnityEngine;
using Vurbiri.EntryPoint;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class LanguageSwitch : VToggleGroup<LanguageItem>
    {
        [SerializeField] private bool _isSave;

        protected override void Start()
        {
            _allowSwitchOff = false;

            var profile = ProjectContainer.Settings.Profile;
            var currentId = profile.Language;
            var languages = profile.Localization.Languages;

            if(languages.Count != _toggles.Count)
                Errors.Message($"[LanguageSwitch] Number of LanguageItem is not equal to the number of Languages ({_toggles.Count} != {languages.Count})");

            for (int index = 0; index < languages.Count; ++index)
                if (_toggles[index].Init(currentId, languages[index]))
                    _activeToggle = _toggles[index];

            _onValueChanged.Init(_activeToggle);
            _onValueChanged.Add(OnValueChanged);

            Transition.OnExit.Add(_onValueChanged.Clear);
        }

        public void ItemsUpdate()
        {
            var currentId = ProjectContainer.Settings.Profile.Language;
            if (_activeToggle != null && _activeToggle.Id != currentId)
            {
                for (int i = _toggles.Count - 1; i >= 0; --i)
                {
                    if (_toggles[i].Id == currentId)
                    {
                        _toggles[i].SilentIsOn = true;
                        break;
                    }
                }
            }
        }

        private void OnValueChanged(LanguageItem item)
        {
            if (item != null)
            {
                var profile = ProjectContainer.Settings.Profile;

                profile.Language = item.Id;
                if (_isSave) 
                    profile.Apply();
            }
        }

        private void OnDestroy()
        {
            _onValueChanged.Clear();
        }
    }
}
