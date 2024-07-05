using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CmButton))]
public class ButtonBuildUniversal : AHinting
{
    [Space]
    [SerializeField] private string _keyGroup = "Berth";
    [SerializeField] private string _keyType = "Berth";
    [SerializeField, ResizableTextArea] private string _format = "<align=\"center\">{0}\r\n<b>{1}</b>";
    [Space]
    [SerializeField] private Image _buttonIcon;
    [SerializeField] private UnityDictionary<CityBuildType, Sprite> _citySprites;

    public bool Interactable { set => _button.interactable = value; }

    private CmButton _button;
    private Graphic _buttonGraphic;
    private Func<string> getText;

    public override void Initialize()
    {
        getText = GetTextSimple;

        base.Initialize();

        _button = _thisSelectable as CmButton;
        _buttonGraphic = _button.targetGraphic;
    }

    protected override void SetText() => _text = getText();

    public void Setup(Crossroad crossroad, Color color)
    {
        CityBuildType cityBuildType = crossroad.BuildType;

        _buttonGraphic.color = color;
        _buttonIcon.sprite = _citySprites[cityBuildType];
        _button.onClick.RemoveAllListeners();

        _keyGroup = cityBuildType.ToString();
        if (cityBuildType == CityBuildType.Upgrade)
        {
            _keyType = crossroad.UpgradeType.ToString();
            getText = GetTextFormat;
        }
        else
        {
            getText = GetTextSimple;
        }

        _text = getText();
    }

    public void AddListener(UnityEngine.Events.UnityAction action) => _button.onClick.AddListener(action);

    private string GetTextSimple() => _localization.GetText(_file, _keyGroup);
    private string GetTextFormat() => string.Format(_format, _localization.GetText(_file, _keyGroup), _localization.GetText(_file, _keyType));
}
