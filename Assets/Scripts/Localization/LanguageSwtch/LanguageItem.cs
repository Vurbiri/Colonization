using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class LanguageItem : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _name;

    private Toggle _toggle;
    private bool _isSave;
    private int _id = -1;
    private Localization _localization;
    private SettingsData _settings;
    
    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _localization = Localization.InstanceF;
        _settings = SettingsData.InstanceF;
    }

    public void Setup(Localization.LanguageType languageType, ToggleGroup toggleGroup, bool isSave) 
    {

        _icon.sprite = languageType.Sprite;
        _name.text = languageType.Name;
        _id = languageType.Id;
        _isSave = isSave;

        _toggle.SetIsOnWithoutNotify(_localization.CurrentId == _id);
        _toggle.group = toggleGroup;
        _toggle.onValueChanged.AddListener(OnSelect);
        _localization.EventSwitchLanguage += OnSwitchLanguage;
    }

    private void OnSelect(bool isOn)
    {
        if(!isOn) return;

        _localization.SwitchLanguage(_id);
        if(_isSave) _settings.Save();
    }

    private void OnSwitchLanguage(Localization localization) => _toggle.SetIsOnWithoutNotify(localization.CurrentId == _id);

    private void OnDestroy()
    {
        if(Localization.Instance != null)
            Localization.Instance.EventSwitchLanguage -= OnSwitchLanguage;
    }
}
