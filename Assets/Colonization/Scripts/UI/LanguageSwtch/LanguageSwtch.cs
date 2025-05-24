using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    [RequireComponent(typeof(VToggleGroup))]
    public class LanguageSwitch : MonoBehaviour
    {
        [SerializeField] private LanguageItem _langPrefab;
        [SerializeField] private VToggleGroup _toggleGroup;
        [Space]
        [SerializeField] private bool _isSave = false;

        public void Init(Colonization.Settings settings)
        {
            _toggleGroup.AllowSwitchOff = false;

            var profile = settings.Profile;
            var languages = profile.Localization.Languages;

            foreach (var item in languages)
                if (!item.Equals(SystemLanguage.Unknown))
                    Instantiate(_langPrefab, transform).Setup(profile, item, _toggleGroup, _isSave);

            Destroy(this);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            //EUtility.SetPrefab(ref _langPrefab);

            if (_toggleGroup == null)
                _toggleGroup = GetComponent<VToggleGroup>();
        }
#endif
    }
}
