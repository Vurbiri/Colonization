using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TextFormatLocalization : TextLocalization
{
    private float _value = 0;

    public void Setup(float value)
    {
        _value = value;
        Setup(Text.text);
    }

    protected override void SetText(Localization localization) => Text.text = string.Format(localization.GetText(_key), _value);
}
