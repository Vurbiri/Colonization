using TMPro;
using UnityEngine;

namespace Vurbiri.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextFormatLocalization : TextLocalization
    {
        private float _value = 0;

        public void Setup(float value)
        {
            _value = value;
            Setup(Text.text);
        }

        protected override void SetText() => Text.text = string.Format(_localization.GetText(_file, _key), _value);
    }
}
