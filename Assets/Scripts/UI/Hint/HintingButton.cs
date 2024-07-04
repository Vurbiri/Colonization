using UnityEngine;

public class HintingButton : AHinting
{
    [SerializeField] private string _key;

    public string Key { get => _key; set { _key = value; SetText(); } }

    protected override void SetText() => _text = _localization.GetText(_file, _key);
}