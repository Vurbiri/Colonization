//Assets\Colonization\Scripts\UI\LanguageSwtch\LanguageItem.cs
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Colonization.Data;
using Vurbiri.Localization;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    [RequireComponent(typeof(Toggle))]
    public class LanguageItem : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _name;

        private Toggle _toggle;
        private bool _isSave;
        private int _id = -1;
        private Language _localization;
        private SettingsData _settings;
        private IUnsubscriber _unsubscriber;

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
            _localization = SceneServices.Get<Language>();
            _settings = SceneServices.Get<SettingsData>();
        }

        public void Setup(LanguageType languageType, ToggleGroup toggleGroup, bool isSave)
        {
            _icon.sprite = languageType.Sprite;
            _name.text = languageType.Name;
            _id = languageType.Id;
            _isSave = isSave;

            _toggle.SetIsOnWithoutNotify(_localization.CurrentId == _id);
            _toggle.group = toggleGroup;
            _toggle.onValueChanged.AddListener(OnSelect);
            _unsubscriber = _localization.Subscribe(OnSwitchLanguage, false);
        }

        private void OnSelect(bool isOn)
        {
            if (!isOn) return;

            _localization.SwitchLanguage(_id);
            if (_isSave) 
                StartCoroutine(_settings.Save_Coroutine());
        }

        private void OnSwitchLanguage(Language localization) => _toggle.SetIsOnWithoutNotify(localization.CurrentId == _id);

        private void OnDestroy()
        {
            _unsubscriber?.Unsubscribe();
        }
    }
}
