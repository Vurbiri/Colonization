using NaughtyAttributes;
using UnityEngine;

public class HintingButtonBuild : AHinting
{
    [Space]
    [SerializeField] private CityGroup _group;
    [SerializeField] private CityType _type;
    [Space]
    [SerializeField, ResizableTextArea] private string _format = "<align=\"center\"><b>{0}</b>\r\n{1}";

    private void Start() => Initialize();

    protected override void SetText() => _text = string.Format(_format, _localization.GetText(_file, _group), _localization.GetText(_file, _type));
}
