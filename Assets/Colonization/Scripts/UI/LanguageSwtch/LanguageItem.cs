//Assets\Colonization\Scripts\UI\LanguageSwtch\LanguageItem.cs
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.TextLocalization;

namespace Vurbiri.Colonization.UI
{
    [RequireComponent(typeof(Toggle))]
    public class LanguageItem : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private Toggle _toggle;

        private bool _isSave;
        private int _id = -1;
        private Profile _profile;

        public void Setup(Profile profile, LanguageType languageType, ToggleGroup toggleGroup, bool isSave)
        {
            _profile = profile;
            _icon.sprite = languageType.Sprite;
            _name.text = languageType.Name;
            _id = languageType.Id;
            _isSave = isSave;

            _toggle.SetIsOnWithoutNotify(_profile.Language == _id);
            _toggle.group = toggleGroup;
            _toggle.onValueChanged.AddListener(OnSelect);
        }

        private void OnSelect(bool isOn)
        {
            if (!isOn) return;

            _profile.Language = _id;
            if (_isSave) _profile.Apply();
        }

#if UNITY_EDITOR
        public void OnValidate()
        {

            if (_toggle == null)
                _toggle = GetComponent<Toggle>();
        }
#endif
    }
}
