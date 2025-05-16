//Assets\Colonization\Scripts\UI\LanguageSwtch\LanguageItem.cs
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    [RequireComponent(typeof(Toggle))]
    public class LanguageItem : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private VToggle _toggle;

        private bool _isSave;
        private SystemLanguage _id;
        private Profile _profile;

        public void Setup(Profile profile, LanguageType languageType, VToggleGroup toggleGroup, bool isSave)
        {
            _profile = profile;
            _icon.sprite = Resources.Load<Sprite>(languageType.SpriteName);
            _name.text = languageType.Name;
            _id = languageType.Id;
            _isSave = isSave;

            _toggle.SilentIsOn = _profile.Language == _id;
            _toggle.Group = toggleGroup;
            _toggle.AddListener(OnSelect);
        }

        private void OnSelect(bool isOn)
        {
            if (!isOn) return;

            _profile.Language = _id;
            if (_isSave) _profile.Apply();
        }

        private void OnDestroy()
        {
            if(_icon.sprite != null)
                Resources.UnloadAsset(_icon.sprite);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_toggle == null)
                _toggle = GetComponent<VToggle>();
        }
#endif
    }
}
