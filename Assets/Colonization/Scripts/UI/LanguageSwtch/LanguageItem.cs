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
        [SerializeField] private TMP_Text _name;

        private Toggle _toggle;
        private bool _isSave;
        private int _id = -1;
        private Settings _settings;

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
            _settings = SceneServices.Get<Settings>();
        }

        public void Setup(LanguageType languageType, ToggleGroup toggleGroup, bool isSave)
        {
            _icon.sprite = languageType.Sprite;
            _name.text = languageType.Name;
            _id = languageType.Id;
            _isSave = isSave;

            _toggle.SetIsOnWithoutNotify(_settings.Language == _id);
            _toggle.group = toggleGroup;
            _toggle.onValueChanged.AddListener(OnSelect);
        }

        private void OnSelect(bool isOn)
        {
            if (!isOn) return;

            _settings.Language = _id;
            if (_isSave) _settings.Apply();
        }
    }
}
